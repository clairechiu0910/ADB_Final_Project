### Project Overview
A website to manage user, projects, target, equipments of star observing. 

### Requirements
* Python 3
* Neo4j graph database
* Visual Studio with Desktop development with c++ workload.

### Restore Nro4j Graph Database
1. Create a new database in neo4j.
2. Open terminal (manage >> open terminal)
3. .\bin\neo4j-admin load --from=[path of database_v2.dump in this project] --database=neo4j --force

### Python Environment
1. Install module
```
pip install astroplan
pip install astropy
```
2. Copy two python file path and python.exe to appsetting.json

