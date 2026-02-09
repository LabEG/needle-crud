# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and project files
COPY LabEG.slnx .
COPY LabEG.NeedleCrud/LabEG.NeedleCrud.csproj LabEG.NeedleCrud/
COPY LabEG.NeedleCrud.Sample/LabEG.NeedleCrud.Sample.csproj LabEG.NeedleCrud.Sample/
COPY LabEG.NeedleCrud.TestsFixtures/LabEG.NeedleCrud.TestsFixtures.csproj LabEG.NeedleCrud.TestsFixtures/

# Restore dependencies
RUN dotnet restore LabEG.NeedleCrud.Sample/LabEG.NeedleCrud.Sample.csproj

# Copy the rest of the source code
COPY LabEG.NeedleCrud/ LabEG.NeedleCrud/
COPY LabEG.NeedleCrud.Sample/ LabEG.NeedleCrud.Sample/
COPY LabEG.NeedleCrud.TestsFixtures/ LabEG.NeedleCrud.TestsFixtures/

# Build and publish the application (self-contained)
WORKDIR /src/LabEG.NeedleCrud.Sample
RUN dotnet publish -c Release -o /app/publish \
    --self-contained true \
    -r linux-musl-x64 \
    /p:PublishSingleFile=true

# Runtime stage (Alpine)
FROM alpine:latest AS runtime
WORKDIR /app

# Install runtime dependencies for self-contained .NET app
RUN apk add --no-cache \
    ca-certificates \
    libgcc \
    libintl \
    libstdc++ \
    icu-libs

# Create a non-root user
RUN addgroup -g 1000 appuser && \
    adduser -u 1000 -G appuser -s /bin/sh -D appuser

# Copy published files from build stage
COPY --from=build /app/publish .

# Set ownership and make executable
RUN chown -R appuser:appuser /app && \
    chmod +x /app/LabEG.NeedleCrud.Sample

# Switch to non-root user
USER appuser

# Expose port
EXPOSE 8080

# Set environment variables
ENV ASPNETCORE_URLS=http://+:8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Start the application
ENTRYPOINT ["./LabEG.NeedleCrud.Sample"]
