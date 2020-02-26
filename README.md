# Blogger Backend API

[TOC]

## Content Negotiation

Support `JSON` and `XML` serialization and deserialization.



## Endpoints

### Blog Posts



#### `POST` `/api/Post`

This endpoint is for create a new blog post.  Request body or data support `JSON` and `XML` formation defined in `Content-Type` header. For invalid body (syntax and format) server will return `400 Bad Request` with error message. Otherwise `201 Created` response will be served in required format.



*JSON Request*

```JSON
{
  "title": "string",
  "body": "string",
  "createdOn": "2020-02-26T11:27:37.982Z"
}
```

*JSON Response*

```JSON
{
  "id": "609964d6-3ab3-446f-980e-08d7baaf36fe",
  "title": "string",
  "body": "string",
  "createdOn": "2020-02-26T11:29:54.129Z",
  "lastUpdateOn": "0001-01-01T00:00:00"
}
```



*XML Request*

```xml
<?xml version="1.0" encoding="UTF-8"?>
<PostDto>
	<Title>string</Title>
	<Body>string</Body>
	<CreatedOn>2020-02-26T11:28:37.660Z</CreatedOn>
</PostDto>
```

*XML Response*

```XML
<Post xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>609964d6-3ab3-446f-980e-08d7baaf36fe</Id>
  <Title>string</Title>
  <Body>string</Body>
  <CreatedOn>2020-02-26T11:29:54.129Z</CreatedOn>
  <LastUpdateOn>0001-01-01T00:00:00</LastUpdateOn>
</Post>
```



#### `GET` `/api/Post`

This endpoint is for get all available posts in database. Response format support `XML` and `JSON`.

*XML Response*

```xml
  <ArrayOfPost xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Post>
      <Id>afb37453-ef7b-49a4-a770-08d7b9fcc1a2</Id>
    <Title>Hello World</Title>
      <Body>First Blog Post</Body>
    <CreatedOn>2020-02-25T14:12:11.506</CreatedOn>
      <LastUpdateOn>0001-01-01T00:00:00</LastUpdateOn>
    </Post>
  </ArrayOfPost>
```
  *JSON Response*

```json
  [ 
    {
      "id": "609964d6-3ab3-446f-980e-08d7baaf36fe",
      "title": "string",
      "body": "string",
      "createdOn": "2020-02-26T11:29:54.129Z",
      "lastUpdateOn": "0001-01-01T00:00:00"
    }
  ]
```



#### `GET` `/api/Post/{id}`

This endpoint take an `Id` as parameter and return the associated `Post` object in `200 OK`  status code . If no post found with this `Id` then server will return `404 Not Found` status code. This `Id` is a `GUID` instance. For any invalid format of `GUID` server will return `400 Bad Request` response. 

*JSON Response*

```json
{
  "id": "609964d6-3ab3-446f-980e-08d7baaf36fe",
  "title": "string",
  "body": "string",
  "createdOn": "2020-02-26T11:29:54.129Z",
  "lastUpdateOn": "0001-01-01T00:00:00"
}
```

*XML Response*

```xml
<Post xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Id>609964d6-3ab3-446f-980e-08d7baaf36fe</Id>
  <Title>string</Title>
  <Body>string</Body>
  <CreatedOn>2020-02-26T11:29:54.129Z</CreatedOn>
  <LastUpdateOn>0001-01-01T00:00:00</LastUpdateOn>
</Post>
```



#### `GET` `/api/Post/page/{number}`

This endpoint allow to paginate through all posts by taking the page number. This page number should be an `Integer` and `number >= 1` otherwise server will return the `number=1` page by default. For overflow `number` server will return an empty array.

*XML Response*

```xml
  <ArrayOfPost xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema">
  <Post>
      <Id>afb37453-ef7b-49a4-a770-08d7b9fcc1a2</Id>
    <Title>Hello World</Title>
      <Body>First Blog Post</Body>
    <CreatedOn>2020-02-25T14:12:11.506</CreatedOn>
      <LastUpdateOn>0001-01-01T00:00:00</LastUpdateOn>
    </Post>
  </ArrayOfPost>
```
  *JSON Response*

```json
  [ 
    {
      "id": "609964d6-3ab3-446f-980e-08d7baaf36fe",
      "title": "string",
      "body": "string",
      "createdOn": "2020-02-26T11:29:54.129Z",
      "lastUpdateOn": "0001-01-01T00:00:00"
    }
  ]
```