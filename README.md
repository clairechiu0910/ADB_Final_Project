## Project Overview
A website to manage user, equipment, project, target for observational astronomy. We used ASP.Net, Vue.js, Neo4j, Python, and [Bootstrap Template](https://demos.creative-tim.com/paper-kit/) to build our application.  
As a user of our website, they have the ability to:

- Basic ability:
  - Register as a user in the system.
  - Add and edit your equipment information.
  - View your own profile and edit information.
	
- Participants ability:
  - Join the project that is recommended.
  - Interest the targets in the project to join the target to the equipment schedule.
  - View other users in the same project in the suggested user page.

- Project manager ability:
  - Edit project
  - Add target to project
  - View project equipments
  
### User Interface
Our system is provided in web-based interfaces, some of our webpages are as shown in this [ppt slide](https://drive.google.com/file/d/1v5peCg5iic34DbAavUgmt0A1CmtoN9e6/view?usp=sharing).



## Set up
### Requirements
* Python 3
* Neo4j graph database
* Visual Studio with Desktop development with c++ workload.

### Restore Nro4j Graph Database
1. Create a new database in neo4j.
2. Open terminal (manage >> open terminal)
3. Execute this
```
.\bin\neo4j-admin load --from=[path of database_v2.dump in this project] --database=neo4j --force
```

### Python Environment
1. Install module
```
pip install astroplan
pip install astropy
```
2. Copy two python file path and python.exe to appsetting.json

