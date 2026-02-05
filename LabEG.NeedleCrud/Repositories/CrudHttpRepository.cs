using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using LabEG.NeedleCrud.Models.Entities;
using LabEG.NeedleCrud.Models.ViewModels.PaginationViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LabEG.NeedleCrud.Repositories;

public class CrudHttpRepository<TEntity, TId> : ICrudHttpRepository<TEntity, TId>
    where TEntity : class, IEntity<TId>, new()
{
    protected string BaseAdress { get; }
    protected string EndPoint { get; }
    protected HttpClient Client { get; }

    public CrudHttpRepository(string baseAddress, string endPoint)
    {
        BaseAdress = baseAddress;
        EndPoint = endPoint;
        Client = new HttpClient() { Timeout = TimeSpan.FromSeconds(10) };
    }

    public async Task<TEntity> Create(TEntity entity)
    {
        return await CreateByUrl(entity, BaseAdress + "/" + EndPoint);
    }

    public async Task<TEntity> CreateByUrl(TEntity entity, string url)
    {
        return await PostAsync(
            url,
            new StringContent(
                JsonConvert.SerializeObject(entity),
                Encoding.UTF8,
                "application/json"
            )
        );
    }

    public async Task Delete(TId id)
    {
        await DeleteAsync(BaseAdress + "/" + EndPoint + "/" + id);
    }

    public async Task<TEntity> GetById(TId id)
    {
        return await GetAsync(BaseAdress + "/" + EndPoint + "/" + id);
    }

    public async Task<IList<TEntity>> GetAll()
    {
        return await GetAllAsync(BaseAdress + "/" + EndPoint);
    }

    public async Task Update(TId id, TEntity entity)
    {
        await PutAsync(
            BaseAdress + "/" + EndPoint + "/" + id,
            new StringContent(
                JsonConvert.SerializeObject(entity),
                Encoding.UTF8,
                "application/json"
            )
        );
    }

    public async Task<PagedList<TEntity>> GetPaged(PagedListQuery query, IQueryable<TEntity> data = null)
    {
        List<string> gets = [];

        if (query.PageNumber > 0)
        {
            gets.Add($"pageNumber={HttpUtility.UrlPathEncode(query.PageNumber.ToString())}");
        }

        if (query.PageSize > 0)
        {
            gets.Add($"pageSize={HttpUtility.UrlPathEncode(query.PageNumber.ToString())}");
        }

        if (query.Sort != null)
        {
            IEnumerable<string> sortRequest = query.Sort.Select((PagedListQuerySort sort) =>
            {
                string direction = sort.Direction == PagedListQuerySortDirection.Desc ? "desc" : "asc";
                return $"{sort.Property}~{direction}";
            });
            gets.Add($"sort={HttpUtility.UrlPathEncode(string.Join(",", sortRequest))}");
        }

        if (query.Filter != null)
        {
            IEnumerable<string> filterRequest = query.Filter.Select((PagedListQueryFilter filter) =>
            {
                string method;
                switch (filter.Method)
                {
                    case PagedListQueryFilterMethod.Less:
                        method = "<";
                        break;

                    case PagedListQueryFilterMethod.LessOrEqual:
                        method = "<=";
                        break;

                    case PagedListQueryFilterMethod.GreatOrEqual:
                        method = ">=";
                        break;

                    case PagedListQueryFilterMethod.Great:
                        method = ">";
                        break;

                    case PagedListQueryFilterMethod.Like:
                        method = "like";
                        break;

                    case PagedListQueryFilterMethod.ILike:
                        method = "ilike";
                        break;

                    case PagedListQueryFilterMethod.Equal:
                    default:
                        method = "=";
                        break;
                }
                return $"{filter.Property}~{method}~{filter.Value}";
            });
            gets.Add($"filter={HttpUtility.UrlPathEncode(string.Join(",", filterRequest))}");
        }

        if (query.Graph != null)
        {
            gets.Add($"graph={HttpUtility.UrlPathEncode(JsonConvert.SerializeObject(query.Graph))}");
        }

        string url = BaseAdress + "/" + EndPoint + "/paged?" + string.Join("&", gets);
        HttpResponseMessage httpResponseMessage = await Client.GetAsync(url);
        CheckResponseStatus(httpResponseMessage);
        string response = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<PagedList<TEntity>>(response);
    }

    public async Task<TEntity> GetGraph(TId id, JObject graph)
    {
        return await GetAsync(
            BaseAdress + "/" + EndPoint + "/" + id + "/graph?" +
            "graph=" + JsonConvert.SerializeObject(graph)
        );
    }

    // handler
    protected void CheckResponseStatus(HttpResponseMessage httpResponseMessage)
    {
        // todo: move bottom, after success status
        string responseBody = httpResponseMessage.Content.ReadAsStringAsync().Result;

        Debug.WriteLine("=== \r\n {0} \r\n === \r\n {1} \r\n ===", JsonConvert.SerializeObject(httpResponseMessage), responseBody);

        if (httpResponseMessage.IsSuccessStatusCode)
        {
            return;
        }

        // message from back
        if (responseBody.IndexOf("{\"message\":") == 0)
        {
            var respMessage = responseBody.Substring(12, responseBody.Length - 2 - 12);
            Debug.WriteLine($"Web exception: \n responce: {responseBody} \n message: {respMessage}");
            throw new Exception(respMessage);
        }

        // nginx exception
        if (false)
        {
        }

        // tomcat exception
        if (responseBody.IndexOf("<") == 0)
        {
            Regex rgx = new("<b>description</b> <u>(.+?)</u>");
            Match match = rgx.Match(responseBody);
            throw new Exception("WebServerException - " + match.Groups[1] ?? httpResponseMessage.ReasonPhrase ?? "Ошибка не указана");
        }

        throw new Exception(httpResponseMessage.ReasonPhrase);
    }

    // DeleteAsync
    protected async Task DeleteAsync(string requestUri, CancellationToken cancellationToken = new CancellationToken())
    {
        HttpResponseMessage httpResponseMessage = await Client.DeleteAsync(requestUri, cancellationToken);
        CheckResponseStatus(httpResponseMessage);
    }

    // GetAsync
    protected async Task<TEntity> GetAsync(
        string requestUri,
        HttpCompletionOption completionOption = new HttpCompletionOption(),
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        HttpResponseMessage httpResponseMessage = await Client.GetAsync(requestUri, completionOption, cancellationToken);
        CheckResponseStatus(httpResponseMessage);
        string response = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TEntity>(response);
    }

    protected async Task<TEntity[]> GetAllAsync(
        string requestUri,
        HttpCompletionOption completionOption = new HttpCompletionOption(),
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        HttpResponseMessage httpResponseMessage = await Client.GetAsync(requestUri, completionOption, cancellationToken);
        CheckResponseStatus(httpResponseMessage);
        string response = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TEntity[]>(response);
    }

    // PostAsync
    protected async Task<TEntity> PostAsync(
        string requestUri,
        HttpContent content = null,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        HttpResponseMessage httpResponseMessage = await Client.PostAsync(requestUri, content, cancellationToken);
        CheckResponseStatus(httpResponseMessage);
        string response = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TEntity>(response);
    }

    // PutAsync
    protected async Task<TEntity> PutAsync(
        string requestUri,
        HttpContent content = null,
        CancellationToken cancellationToken = new CancellationToken()
    )
    {
        HttpResponseMessage httpResponseMessage = await Client.PutAsync(requestUri, content, cancellationToken);
        CheckResponseStatus(httpResponseMessage);
        string response = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<TEntity>(response);
    }
}