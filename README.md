<figure>
<img src="http://www.hypergrid.com/wp-content/themes/hypergrid/img/logo.png" alt="" />
</figure>

This project includes the source code to a simple Microsoft .NET application called "Names Directory" that allows users to view, add and remove names stored on a connected database. 

The purpose of this project is to provide detailed steps for running Microsoft .NET applications on Docker containers. The Dockerfile included in this project allows users to build their custom image. However, this application also enables users to leverage **environment variables** to specify the database connection details and to use **dotnet ef database update** to automatically initialize the connected database - even if it's initially empty.

To run & manage thisDocker .NET "Names Directory" application on 18 different clouds and virtualization platforms (including HyperGrid, Hyper-V, vSphere, OpenStack, AWS, Rackspace, Microsoft Azure, DigitalOcean, IBM SoftLayer, etc.), make sure that you either:
-   **Sign Up for HyperForm SaaS** -- <http://dchq.io>, or
-   **Download HyperForm On-Premise Standard Edition for free** -- <http://dchq.co/hyperform-on-premise.html>

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ)


Customize & Run all the published Docker .NET application templates and many other templates (including multi-tier Java application stacks, Python, Ruby, PHP, Mongo Replica Set Cluster, Drupal, Wordpress, MEAN.JS, etc.)

### .NET with Embedded-SQLite

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ&bl=2c91808656b653150156c9e7be6865e9)

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
dotnet:
  image: dchq/dotnet-names-directory:latest
  publish_all: true
  #ports:
  #  - 5000:5000
  host: host1
  mem_min: 200m
  environment:
    - database_driverClassName=sqlight
    - database_url=Filename=.\\sqlight_db.db
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


### 2-Tier .NET (Nginx-Embedded-SQLite)

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ&bl=2c91808656b653150156c9e2e62f6573)

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
nginx:
  image: nginx:latest
  publish_all: true
  mem_min: 50m
  host: host1
  plugins:
    - !plugin
      id: 0H1Nk
      restart: true
      lifecycle: on_create, post_scale_out:dotnet, post_scale_in:dotnet
      arguments:
        # Use container_private_ip if you're using Docker networking
        - servers=server {{dotnet | container_private_ip}}:5000;
        # Use container_hostname if you're using Weave networking
        #- servers=server {{dotnet | container_hostname}}:5000;
dotnet:
  image: dchq/dotnet-names-directory:latest
  host: host1
  mem_min: 200m
  environment:
    - database_driverClassName=sqlight
    - database_url=Filename=.\\sqlight_db.db
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### 2-Tier .NET (Apache HTTP-Embedded-SQLite)

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ&bl=2c91808756b64ee70156c9e631aa662d)

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
http-lb:
  image: httpd:latest
  publish_all: true
  mem_min: 50m
  host: host1
  plugins:
    - !plugin
      id: uazUi
      restart: true
      lifecycle: on_create, post_scale_out:dotnet, post_scale_in:dotnet
      arguments:
        # Use container_private_ip if you're using Docker networking
        - BalancerMembers=BalancerMember http://{{dotnet | container_private_ip}}:5000
        # Use container_hostname if you're using Weave networking
        #- BalancerMembers=BalancerMember http://{{dotnet | container_hostname}}:5000
dotnet:
  image: dchq/dotnet-names-directory:latest
  host: host1
  mem_min: 200m
  environment:
    - database_driverClassName=sqlight
    - database_url=Filename=.\\sqlight_db.db
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### 2-Tier .NET with PostgreSQL

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ&bl=2c91808756b64ee70156c9df2b3765a9)

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
dotnet:
  image: dchq/dotnet-names-directory:latest
  publish_all: true
  #ports:
  #  - 5000:5000
  host: host1
  mem_min: 200m
  environment:
    - database_driverClassName=postgres
    - database_url=Host={{postgres | container_private_ip}};Database={{postgres | POSTGRES_DB}};Username={{postgres | POSTGRES_USER}};Password={{postgres | POSTGRES_PASSWORD}}
  plugins:
    - !plugin
      id: hHjF0
      lifecycle: post_create
      restart: false
postgres:
  image: postgres:latest
  host: host1
  mem_min: 300m
  environment:
    - POSTGRES_DB=NameDirectory
    - POSTGRES_USER=root
    - POSTGRES_PASSWORD={{alphanumeric | 8}}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


### 3-Tier .NET (Nginx-.NET-PostgreSQL)

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ&bl=2c91808656b653150156c9995f205f81)

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
nginx:
  image: nginx:latest
  publish_all: true
  mem_min: 50m
  host: host1
  plugins:
    - !plugin
      id: 0H1Nk
      restart: true
      lifecycle: on_create, post_scale_out:dotnet, post_scale_in:dotnet
      arguments:
        # Use container_private_ip if you're using Docker networking
        - servers=server {{dotnet | container_private_ip}}:5000;
        # Use container_hostname if you're using Weave networking
        #- servers=server {{dotnet | container_hostname}}:5000;
dotnet:
  image: dchq/dotnet-names-directory:latest
  host: host1
  mem_min: 200m
  environment:
    - database_driverClassName=postgres
    - database_url=Host={{postgres | container_private_ip}};Database={{postgres | POSTGRES_DB}};Username={{postgres | POSTGRES_USER}};Password={{postgres | POSTGRES_PASSWORD}}
  plugins:
    - !plugin
      id: hHjF0
      lifecycle: post_create
      restart: false
postgres:
  image: postgres:latest
  host: host1
  mem_min: 300m
  environment:
    - POSTGRES_DB=NameDirectory
    - POSTGRES_USER=root
    - POSTGRES_PASSWORD={{alphanumeric | 8}}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### 3-Tier .NET (Apache HTTP-.NET-PostgreSQL)

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ&bl=2c91808656b653150156c9dd59ee650d)

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
http-lb:
  image: httpd:latest
  publish_all: true
  mem_min: 50m
  host: host1
  plugins:
    - !plugin
      id: uazUi
      restart: true
      lifecycle: on_create, post_scale_out:dotnet, post_scale_in:dotnet
      arguments:
        # Use container_private_ip if you're using Docker networking
        - BalancerMembers=BalancerMember http://{{dotnet | container_private_ip}}:5000
        # Use container_hostname if you're using Weave networking
        #- BalancerMembers=BalancerMember http://{{dotnet | container_hostname}}:5000
dotnet:
  image: dchq/dotnet-names-directory:latest
  host: host1
  mem_min: 200m
  environment:
    - database_driverClassName=postgres
    - database_url=Host={{postgres | container_private_ip}};Database={{postgres | POSTGRES_DB}};Username={{postgres | POSTGRES_USER}};Password={{postgres | POSTGRES_PASSWORD}}
  plugins:
    - !plugin
      id: hHjF0
      lifecycle: post_create
      restart: false
postgres:
  image: postgres:latest
  host: host1
  mem_min: 300m
  environment:
    - POSTGRES_DB=NameDirectory
    - POSTGRES_USER=root
    - POSTGRES_PASSWORD={{alphanumeric | 8}}
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


### .NET Connecting to MS SQL Server

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ&bl=2c91808656b653150156cd1e67142023)

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
dotnet:
  image: dchq/dotnet-names-directory:latest
  publish_all: true
  #ports:
  #  - 5000:5000
  host: host1
  mem_min: 200m
  environment:
    - database_driverClassName=mssql
    - database_url=Server=tcp:dchq-sql-server.database.windows.net,1433;Initial Catalog=NameDirectory;Persist Security Info=False;User ID={your_username};Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


### 2-Tier .NET (Nginx Connecting to MS SQL Server)

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ&bl=2c91808656b653150156cd51951d23d1)

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
nginx:
  image: nginx:latest
  publish_all: true
  mem_min: 50m
  host: host1
  plugins:
    - !plugin
      id: 0H1Nk
      restart: true
      lifecycle: on_create, post_scale_out:dotnet, post_scale_in:dotnet
      arguments:
        # Use container_private_ip if you're using Docker networking
        - servers=server {{dotnet | container_private_ip}}:5000;
        # Use container_hostname if you're using Weave networking
        #- servers=server {{dotnet | container_hostname}}:5000;
dotnet:
  image: dchq/dotnet-names-directory:latest
  publish_all: false
  #ports:
  #  - 5000:5000
  host: host1
  mem_min: 200m
  environment:
    - database_driverClassName=mssql
    - database_url=Server=tcp:dchq-sql-server.database.windows.net,1433;Initial Catalog=NameDirectory;Persist Security Info=False;User ID={your_username};Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### 2-Tier .NET (Apache HTTP Connecting to MS SQL Server)

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ&bl=2c91808656b653150156cd52a12023e1)

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
http-lb:
  image: httpd:latest
  publish_all: true
  mem_min: 50m
  host: host1
  plugins:
    - !plugin
      id: uazUi
      restart: true
      lifecycle: on_create, post_scale_out:dotnet, post_scale_in:dotnet
      arguments:
        # Use container_private_ip if you're using Docker networking
        - BalancerMembers=BalancerMember http://{{dotnet | container_private_ip}}:5000
        # Use container_hostname if you're using Weave networking
        #- BalancerMembers=BalancerMember http://{{dotnet | container_hostname}}:5000
dotnet:
  image: dchq/dotnet-names-directory:latest
  publish_all: false
  #ports:
  #  - 5000:5000
  host: host1
  mem_min: 200m
  environment:
    - database_driverClassName=mssql
    - database_url=Server=tcp:dchq-sql-server.database.windows.net,1433;Initial Catalog=NameDirectory;Persist Security Info=False;User ID={your_username};Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~


