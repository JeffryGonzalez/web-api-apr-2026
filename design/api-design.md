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
    "name": "Microsoft",
    "site": "https://www.microsoft.com",
    "pointOfContact": {
        "name": "Satya Nadella",
        "email": "satya@microsoft.com",
        "phone": "888-5555"
    }
   
}

```



## Representations

### Policies
