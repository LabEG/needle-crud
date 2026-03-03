#!/usr/bin/env bash
# ---------------------------------------------------------------------------
# run-tests-coverage.sh
# Runs all tests with code coverage and generates an HTML report.
#
# Output:
#   LabEG.NeedleCrud.Tests/TestResults/   – raw coverage XML (per-run GUIDs)
#   LabEG.NeedleCrud.Tests/coverage-report/ – HTML report (index.html)
# ---------------------------------------------------------------------------
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
TEST_PROJECT="$SCRIPT_DIR/LabEG.NeedleCrud.Tests"
TEST_CSPROJ="$TEST_PROJECT/LabEG.NeedleCrud.Tests.csproj"
REPORT_DIR="$TEST_PROJECT/coverage-report"
RAW_DIR="$REPORT_DIR/TestResults"

# ---------------------------------------------------------------------------
# 1. Clean previous results
# ---------------------------------------------------------------------------
echo "==> Cleaning previous test results..."
rm -rf "$REPORT_DIR"
mkdir -p "$RAW_DIR"

# ---------------------------------------------------------------------------
# 2. Run tests with XPlat coverage collector (coverlet)
#    Run against every target framework defined in the project.
# ---------------------------------------------------------------------------
echo "==> Running tests with coverage..."
dotnet test "$TEST_CSPROJ" \
    --configuration Release \
    --collect:"XPlat Code Coverage" \
    --results-directory "$RAW_DIR" \
    --verbosity normal

# ---------------------------------------------------------------------------
# 3. Ensure reportgenerator is available
# ---------------------------------------------------------------------------
if ! command -v reportgenerator &>/dev/null; then
    echo "==> Installing dotnet-reportgenerator-globaltool..."
    dotnet tool install -g dotnet-reportgenerator-globaltool
    # Reload PATH so the newly installed tool is found in the same session
    export PATH="$PATH:$HOME/.dotnet/tools"
fi

# ---------------------------------------------------------------------------
# 4. Generate HTML report from all collected coverage files
# ---------------------------------------------------------------------------
echo "==> Generating HTML coverage report..."

# reportgenerator is a Windows binary; it cannot resolve Unix-style /d/...
# paths. Convert every path to Windows format with cygpath before passing them.
COVERAGE_FILES="$(find "$RAW_DIR" -name "coverage.cobertura.xml" \
    | while IFS= read -r f; do cygpath -w "$f"; done \
    | tr '\n' ';' | sed 's/;$//')"

if [[ -z "$COVERAGE_FILES" ]]; then
    echo "ERROR: No coverage.cobertura.xml files found under $RAW_DIR" >&2
    exit 1
fi

REPORT_DIR_WIN="$(cygpath -w "$REPORT_DIR")"

echo "==> Coverage files found: $(echo "$COVERAGE_FILES" | tr ';' '\n' | wc -l)"

reportgenerator \
    -reports:"$COVERAGE_FILES" \
    -targetdir:"$REPORT_DIR_WIN" \
    -reporttypes:"Html;TextSummary" \
    -assemblyfilters:"+LabEG.NeedleCrud" \
    -verbosity:Warning

# ---------------------------------------------------------------------------
# 5. Print summary and path
# ---------------------------------------------------------------------------
if [[ -f "$REPORT_DIR/Summary.txt" ]]; then
    echo ""
    echo "--- Coverage Summary ---"
    cat "$REPORT_DIR/Summary.txt"
fi

echo ""
echo "==> HTML report: $REPORT_DIR/index.html"
