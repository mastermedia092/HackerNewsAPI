
# Hacker News Best Stories API

## Enhancements
- **Error Handling**: Skips stories that fail to fetch and logs errors for debugging.
- **Pagination**: Supports `page` and `pageSize` query parameters for paginated results.
- **Metadata in Response**: Adds total stories, page, and page size in the API response.
- **Enhanced Caching**: Caches individual story details to reduce repetitive API calls.
- **Optimized Performance**: Fetches stories in smaller batches to optimize response time.

## Prerequisites
- .NET 8 SDK or later

## Setup Instructions

1. **Clone the Repository:**
   ```bash
   git clone <repository-url>
   cd <repository-directory>
   ```

2. **Restore Dependencies:**
   ```bash
   dotnet restore
   ```

3. **Run the Application:**
   ```bash
   dotnet run
   ```

4. **Access Swagger for API Testing:**
    - Navigate to `http://localhost:5000/swagger` in your browser.

## API Endpoint

### `GET /api/stories/top`
Retrieve the top `n` stories from Hacker News, sorted by score.

#### Query Parameters:
- **`page`**: (optional) The page number to retrieve (default: 1).
- **`pageSize`**: (optional) Number of stories per page (default: 10).

#### Example Request:
```bash
curl "http://localhost:5000/api/stories/top?page=1&pageSize=5"
```

#### Example Response:
```json
{
    "totalStories": 100,
    "page": 1,
    "pageSize": 5,
    "stories": [
        {
            "title": "A uBlock Origin update was rejected from the Chrome Web Store",
            "uri": "https://github.com/uBlockOrigin/uBlock-issues/issues/745",
            "postedBy": "ismaildonmez",
            "time": "2019-10-12T13:43:01+00:00",
            "score": 1757,
            "commentCount": 588
        },
        { ... }
    ]
}
```

## Improvements
- **Distributed Caching**: Implement Redis for better scalability.
- **Rate Limiting**: Add rate limiting to prevent abuse.
- **Testing**: Write comprehensive unit and integration tests for services and controllers.

## License
This project is licensed under the MIT License.
