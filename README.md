# Distributed Web Platform

## Deployment Diagram

This laboratory work uses Docker and Docker-Compose for deploying infrastructure as shown in following image.
![Deployment Diagram](/images/deployment_diagram.png)

### Nodes

#### Load balancer

Load balancer is connected to:

- *Redis* - this service is used for caching. Caching is used only for GET methods. When we receive a different method on some resource, ex.`/api/actors `we invalidate cache for all entries related to `/api/actors` because most probably these are not more relevant.

- *MessageBroker*(RabbitMq). - this service is used for discovery of web servers. When a server is starting up, it sends a message via MessageBroker about it's state and *ip:port* listening.

Consuming all messages from *MessageBroker* we have a list of existing Web Servers and we do **Round robin** balancing between servers.

##### Example for load balancer
Here we can find 3 requests.

- First request - GET - is redirected to `172.18.0.8` and cached response

- Second request - GET - is not redirected, but it's response is obtained from *Redis caching*

- Third request - POST - as it is modifying resource request, we clear cache for pattern *api/actor* and redirect request to next node, `172.18.0.7`. We can see here round robin. Next unchached/unchacheable request will be handled again by `172.18.0.8`

```text
loadbalancer_1   | Received a request. Redirecting to http://172.18.0.8/api/actor
loadbalancer_1   | Returning cached request.
loadbalancer_1   | Invalidating cache for api/actor because of method : POST
loadbalancer_1   | Received a request. Redirecting to http://172.18.0.7/api/actor
```

#### MessageBroker

Message broker is a instance of `RabbitMQ` used for `publish/subscribe` pattern. This broker is used for followings:

- Server state event. Registration to load balancer (published by `web` and consumed by `loadbalancer`)

- Synchronization. When a entity changes in one `web`-`sql` instance, we send event with modifications to other's `web`-`sql`. 

##### Example

In following example we see that entity was added via REST on `web_md` instance. This instance created new event and published to message broker. This event was dispatched to all subscribers

- `web_md` ignored it as it was published by himself

- `web_us` handled it and updated created entity to own database.

```text
web_md_1         | dbug: WebApplication.Data.Repository.IEventSynchronizer[0]
web_md_1         |       Publish insert event for 039231fd-9598-45b9-8f98-ea64fb14be6c
web_md_1         | dbug: WebApplication.Data.Repository.IEventSynchronizer[0]
web_md_1         |       Received insert event for 039231fd-9598-45b9-8f98-ea64fb14be6c
web_md_1         | dbug: WebApplication.Data.Repository.IEventSynchronizer[0]
web_md_1         |       Skipping because emitter server id matches current
web_us_1         | dbug: WebApplication.Data.Repository.IEventSynchronizer[0]
web_us_1         |       Received insert event for 039231fd-9598-45b9-8f98-ea64fb14be6c
```

#### Redis

Redis is used just for key/value cache store. Here we store GET requests and removing them when POST/PUT/DELETE request is coming.

#### Web

This is a ASP.NET Core 2.0 REST application. It has 2 resources:

- `/api/actor` - an actor
- `/api/movie` - a movie

These resources support all methods

- `get`
- `post`
- `put`
- `delete`

These resources also supports `HATEOAS` as shown in example.

##### Example GET /api/actor

```json
[
    {
        "id": "039231fd-9598-45b9-8f98-ea64fb14be6c",
        "firstName": "Nicu",
        "lastName": "Maxian",
        "movies": [],
        "_links": {
            "get": {
                "rel": "Actor/GetActorById",
                "href": "http://localhost:8080/api/Actor/039231fd-9598-45b9-8f98-ea64fb14be6c",
                "method": "GET"
            },
            "update": {
                "rel": "Actor/PutActor",
                "href": "http://localhost:8080/api/Actor/039231fd-9598-45b9-8f98-ea64fb14be6c",
                "method": "PUT"
            },
            "delete": {
                "rel": "Actor/DeleteActor",
                "href": "http://localhost:8080/api/Actor/039231fd-9598-45b9-8f98-ea64fb14be6c",
                "method": "DELETE"
            }
        }
    }
]
```


##### Example GET /api/movie

```json
[
    {
        "id": "17f43e1f-ab3f-4143-8ee7-e917c27cb925",
        "title": "Forsaj 344",
        "releasedYear": 2121,
        "sales": 123456,
        "rating": 143.3,
        "actors": [],
        "_links": {
            "get": {
                "rel": "Movie/GetMovieById",
                "href": "http://localhost:8080/api/Movie/17f43e1f-ab3f-4143-8ee7-e917c27cb925",
                "method": "GET"
            },
            "update": {
                "rel": "Movie/PutMovie",
                "href": "http://localhost:8080/api/Movie/17f43e1f-ab3f-4143-8ee7-e917c27cb925",
                "method": "PUT"
            },
            "delete": {
                "rel": "Movie/DeleteMovie",
                "href": "http://localhost:8080/api/Movie/17f43e1f-ab3f-4143-8ee7-e917c27cb925",
                "method": "DELETE"
            }
        }
    },
    {
        "id": "1fc80b56-b1bc-4746-80e4-0dfa773a717a",
        "title": "Forsaj 344",
        "releasedYear": 2121,
        "sales": 123456,
        "rating": 143.3,
        "actors": [],
        "_links": {
            "get": {
                "rel": "Movie/GetMovieById",
                "href": "http://localhost:8080/api/Movie/1fc80b56-b1bc-4746-80e4-0dfa773a717a",
                "method": "GET"
            },
            "update": {
                "rel": "Movie/PutMovie",
                "href": "http://localhost:8080/api/Movie/1fc80b56-b1bc-4746-80e4-0dfa773a717a",
                "method": "PUT"
            },
            "delete": {
                "rel": "Movie/DeleteMovie",
                "href": "http://localhost:8080/api/Movie/1fc80b56-b1bc-4746-80e4-0dfa773a717a",
                "method": "DELETE"
            }
        }
    },
    {
        "id": "6a336e92-9ce2-49d7-8ef9-588889e07dff",
        "title": "Forsaj 344",
        "releasedYear": 2121,
        "sales": 123456,
        "rating": 143.3,
        "actors": [],
        "_links": {
            "get": {
                "rel": "Movie/GetMovieById",
                "href": "http://localhost:8080/api/Movie/6a336e92-9ce2-49d7-8ef9-588889e07dff",
                "method": "GET"
            },
            "update": {
                "rel": "Movie/PutMovie",
                "href": "http://localhost:8080/api/Movie/6a336e92-9ce2-49d7-8ef9-588889e07dff",
                "method": "PUT"
            },
            "delete": {
                "rel": "Movie/DeleteMovie",
                "href": "http://localhost:8080/api/Movie/6a336e92-9ce2-49d7-8ef9-588889e07dff",
                "method": "DELETE"
            }
        }
    }
]
```

#### Sql

As a SQL is used `PostGres-10` because it is reliable and powerful open source database.

## Usage

Because this project uses docker and docker-compose, entire platform can be run with just one command

```text
docker-compose up
```

Will open a port `8080` to load balancer. You will be able to access resources only by this port. *Web application ports are not exposed to public*.