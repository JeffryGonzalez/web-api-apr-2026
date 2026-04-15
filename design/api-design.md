# Software Center API Design

## Resources
- "An important thingy with a name"
- the name is a URI - 
    - e.g. https://api.company.com/catalog 

    - https - "scheme" (http | https)
    - api.company.com - "authority" - "origin" - "server"
    - "software-center/catalog" - "path" to the resource.


- Our Resources:
    - /catalog 
        - a collection (array?) of all the software we support.
        - subordinate resources
            /catalog/938938938
            /catalog/39839839
            /catalog/39839839/employees-entitled

    - /vendors
        - a collection of vendors

### Operations 

- add a catalog item
- add a vendor (POST)
    - send us:

```http
POST https://localhost:1338/vendors
Content-Type: application/json

{
    "name": "JasperFx",
    "site": "https://www.jasperfx.com",
    "pointOfContact": {
        "name": "Jeremy miller",
        "email": "jeremey@jasperfx.com",
        "phone": "888-5555"
    }
   
}

```

```http
GET https://localhost:1338/vendors/143a4802-e3d3-46fe-9167-
```


```http
GET https://localhost:1338/vendors
```
## Representations


1. You have the "Real Thing" - like a policy, like an order, like an appointment. The business thing.
2. Those things will have one or more representations.
3. Another representation is the "thing" that you use to create the thing.


### To create a Vendor, give us one of these:

### Request Representation
 
```json
{
    "name": "Microsoft",
    "site": "https://www.microsoft.com",
    "pointOfContact": {
        "name": "Satya Nadella",
        "email": "satya@microsoft.com",
        "phone": "888-5555"
    }
}
```


### "The Business Entity" - the Real Thing.

1. This could be an `INSERT INTO Vendors {...}`
2. For ours, I am going to store it sort of like what the response model is, but also put in the ID of the
   manager that created it.


```json
{
    "id": "{guid}",
    "name": "Microsoft",
    "added": "{dtoffset}",
    "site": "https://www.microsoft.com",
    "createdBy": "{sub}",
    "pointOfContact": {
        "name": "Satya Nadella",
        "email": "satya@microsoft.com",
        "phone": "888-5555"
    }
}
```

### Response Representation

```json
{
    "id": "{guid}",
    "name": "Microsoft",
    "added": "{dtoffset}",
    "site": "https://www.microsoft.com",
    "pointOfContact": {
        "name": "Satya Nadella",
        "email": "satya@microsoft.com",
        "phone": "888-5555"
    }
}
```

### Policies
