<figure>
<img src="http://www.hypergrid.com/wp-content/themes/hypergrid/img/logo.png" alt="" />
</figure>

DCHQ - Docker .NET Example 
===========================

This project includes the source code to a simple Microsoft .NET application called "Names Directory" that allows users to view, add and remove names stored on a connected database. 

The purpose of this project is to provide detailed steps for running Microsoft .NET applications on Docker containers. The Dockerfile included in this project allows users to build their custom image. However, this application also enables users to leverage **environment variables** to specify the database connection details and to use **dotnet ef database update** to automatically initialize the connected database - even if it's initially empty.

To run & manage this Docker .NET "Names Directory" application on 18 different clouds and virtualization platforms (including HyperGrid, Hyper-V, vSphere, OpenStack, AWS, Rackspace, Microsoft Azure, DigitalOcean, IBM SoftLayer, etc.), make sure that you either:
-   **Sign Up for HyperForm SaaS** -- <http://dchq.io>, or
-   **Download HyperForm On-Premise Standard Edition for free** -- <http://dchq.co/hyperform-on-premise.html>

[![Customize and Run](https://dl.dropboxusercontent.com/u/4090128/dchq-customize-and-run.png)](https://www.dchq.io/landing/products.html#/library?org=DCHQ)

Customize & Run all the published Docker .NET application templates and many other templates (including multi-tier Java application stacks, Python, Ruby, PHP, Mongo Replica Set Cluster, Drupal, Wordpress, MEAN.JS, etc.)

**Table of Contents**  

- [A Step by Step Guide for Dockerizing a .NET Application that Connects to SQLite, PostgreSQL and Microsoft SQL Server](#dchq---docker-java-example-)
		- [.NET with Embedded-SQLite)](#3-tier-java-nginx--tomcat--mysql)
		- [2-Tier .NET (Nginx-Embedded-SQLite)](#3-tier-java-nginx--jetty--mysql)
		- [2-Tier .NET (Apache HTTP-Embedded-SQLite)](#3-tier-java-nginx--jboss--mysql)
		- [2-Tier .NET with PostgreSQL](#2-tier-java-websphere--mysql)
		- [3-Tier .NET (Nginx-.NET-PostgreSQL)](#3-tier-java-nginx--tomcat--postgresql)
		- [3-Tier .NET (Apache HTTP-.NET-PostgreSQL)](#3-tier-java-nginx--jetty--postgresql)
		- [.NET Connecting to MS SQL Server](#3-tier-java-nginx--jboss--postgresql)
		- [2-Tier .NET (Nginx Connecting to MS SQL Server)](#2-tier-java-websphere--postgresql)
		- [2-Tier .NET (Apache HTTP Connecting to MS SQL Server)](#3-tier-java-nginx--tomcat--oracle-xe)
		- [Environment Variable Bindings Across Images](#invoking-a-plug-in-to-initialize-the-database-separately-on-a-3-tier-java-nginx--tomcat--mysql)
		- [Plug-ins to Configure Web Servers and .NET Servers at Request Time & Post-Provision](#invoking-a-plug-in-to-initialize-the-database-separately-on-a-3-tier-java-nginx--tomcat--mysql)

 

A Step by Step Guide for Dockerizing a .NET Application that Connects to SQLite, PostgreSQL and Microsoft SQL Server
=======================================================================================================================================


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
  plugins:
    - !plugin
      id: hHjF0
      lifecycle: post_create
      restart: false
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
  plugins:
    - !plugin
      id: hHjF0
      lifecycle: post_create
      restart: false
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
  plugins:
    - !plugin
      id: hHjF0
      lifecycle: post_create
      restart: false
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

### Environment Variable Bindings Across Images

A user can create cross-image environment variable bindings by making a reference to another image’s environment variable or container values. For example, the **database_url** environment variable in the .NET container is referencing the container private IP, database name, database username, and database password of the PostgreSQL container in order allow .NET to establish a connection with the database --  database_url=Host={{postgres | container_private_ip}};Database={{postgres | POSTGRES_DB}};Username={{postgres | POSTGRES_USER}};Password={{postgres | POSTGRES_PASSWORD}}

The main advantage here is that users do not have to hard-code these values and thus preventing container name or port conflicts. Moreover, users can randomize sensitive information (like passwords) using {{alphanumeric | 8}} closures and pass this information to other containers at run-time without worrying about visibility to passwords.

Here is a list of supported environment variable values:
-   **{{alphanumeric | 8}}** – creates a random 8-character alphanumeric string. This is most useful for creating random passwords.
-   **{{Image Name | ip}}** – allows you to enter the host IP address of a container as a value for an environment variable. This is most useful for allowing the middleware tier to establish a connection with the database.
-   **{{Image Name | container_hostname}}** or **{{Image Name | container_ip}}** – allows you to enter the name of a container as a value for an environment variable. This is most useful for allowing the middleware tier to establish a secure connection with the database (without exposing the database port).
-   **{{Image Name | container_private_ip}}** – allows you to enter the internal IP of a container as a value for an environment variable. This is most useful for allowing the middleware tier to establish a secure connection with the database (without exposing the database port).
-   **{{Image Name | port_Port Number}}** – allows you to enter the Port number of a container as a value for an environment variable. This is most useful for allowing the middleware tier to establish a connection with the database. In this case, the port number specified needs to be the internal port number – i.e. not the external port that is allocated to the container. For example, {{PostgreSQL | port_5432}} will be translated to the actual external port that will allow the middleware tier to establish a connection with the database.
-   **{{Image Name | Environment Variable Name}}** – allows you to enter the value an image’s environment variable into another image’s environment variable. The use cases here are endless – as most multi-tier applications will have cross-image dependencies.

### Plug-ins to Configure Web Servers and .NET Servers at **Request Time & Post-Provision**

Across all these application templates, you will notice that some of the containers are invoking BASH script plug-ins at different life-cycle stages in order to configure the container.

These plug-ins can be created by navigating to **Blueprints > Plug-ins**. Once the BASH script is provided, the HyperForm agent will execute this script **inside the container**. A user can specify arguments that can be overridden at request time and post-provision. Anything preceded by the **$** sign is considered an argument -- for example, **$zip_url** can be an argument that allows developers to specify the download URL for a ZIP file. This can be overridden at request time and post-provision.

The plug-in ID needs to be provided when defining the YAML-based application template. For example, to invoke a BASH script plug-in for Nginx, we would reference the plug-in ID as follows:
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
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

In the example templates, we are invoking 3 BASH script plug-ins.

-   **Nginx** plug-in ID is **0H1Nk**.
-   **Apache HTTP Server (httpd)** plug-in ID is **uazUi**.
-   **.NET Database Initialization** plug-in ID is **hHjF0**

The service discovery framework in HyperForm provides event-driven life-cycle stages that executes custom scripts to re-configure application components. This is critical when scaling out clusters for which a load balancer may need to be re-configured or a replica set may need to be re-balanced.

You will notice that the Nginx and Apache HTTP plug-ins are getting executed during these different stages or events:

-   **When the Nginx or the Apache HTTP container is created** -- in this case, the container IP's of the application servers are injected into the default configuration file to facilitate the load balancing to the right services

-   **When the ASP.NET application server cluster is scaled in or scale out** -- in this case, the updated container IP's of the application servers are injected into the default configuration file to facilitate the load balancing to the right services

-   **When the ASP.NET application servers are stopped or started** -- in this case, the updated container IP's of the application servers are injected into the default configuration file to facilitate the load balancing to the right services

So the service discovery framework here is doing both service registration (by keeping track of the container IP's and environment variable values) and service discovery (by executing the right scripts during certain events or stages).

The lifecycle parameter in plug-ins allows you to specify the exact stage or event to execute the plug-in. If no lifecycle is specified, then by default, the plug-in will be execute on_create. Here are the supported lifecycle stages:

-   **on_create** -- executes the plug-in when creating the container
-   **on_start** -- executes the plug-in after a container starts
-   **on_stop** -- executes the plug-in before a container stops
-   **on_destroy** -- executes the plug-in before destroying a container
-   **pre_create** – executes the plug-in before creating the container
-   **post_create** -- executes the plug-in after the container is created and running
-   **post_start[:Node]** -- executes the plug-in after another container starts
-   **post_stop[:Node]** -- executes the plug-in after another container stops
-   **post_destroy[:Node]** -- executes the plug-in after another container is destroyed
-   **post_scale_out[:Node]** -- executes the plug-in after another cluster of containers is scaled out
-   **post_scale_in[:Node]** -- executes the plug-in after another cluster of containers is scaled in
-   **cron(0 1 1 * * ?)** – schedules the plug-in based on a specified cron expression. Here are some examples for cron expressions.
-   **exec_on_machine** – executes the plug-in on the underlying machine. This lifecycle can be used with other container life cycles. For example, exec_on_machine pre_create will execute the plug-in on the machine before creating the container.

In order to **automatically initialize a connected database**, the .NET container needs to run **dotnet ef database update** post-provision -- as only then is the .NET container aware of the database connection details that are passed using **environment variables**. Of course this plug-in will not be needed if the database connection details are hard-coded in the **appsettings.json** file.

<https://github.com/dchqinc/docker-dotnet/blob/master/appsettings.json>

With the exception of the embedded SQLite application example, all other application templates that are connected to either PostgreSQL or Microsoft SQL Server are invoking the exact same BASH script plug-in (plug-in ID: **hHjF0**).

Here's an example of how this plug-in is being invoked:

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
  plugins:
    - !plugin
      id: hHjF0
      lifecycle: post_create
      restart: false
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

The BASH script plug-in was created by navigating to **Blueprints** > **Plug-ins** and looks something like this:

~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
#!/bin/bash

dotnet restore
dotnet ef database update
dotnet run
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~



