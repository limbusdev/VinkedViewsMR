# Server

## Voraussetzungen

* Anwendungen
    * MongoDB 4.\*
    * Node.js 10.\*
* NPM
    * mongoose
    * express

## Verwendung

1. mangod.exe starten
2. in anderem Terminal mango.exe starten
3. node Main.js

Dann sollte alles korrekt laufen.

## Links

* [NodeJS und MongoDB](https://codeburst.io/the-only-nodejs-introduction-youll-ever-need-d969a47ef219)
* [MongoDB Schema Beispiel](https://mongoosejs.com/docs/schematypes.html)


# Notizen

* alles im Ordner **public** kann automatisch vom Browser angefordert werden




* Query string: request.url contains trailing address --> for localhost:8080/test it contains /test
* Parsing the Query string: url.parse(request.url, true).query returns the query-Object, containing all included variables

**Example:**

For 'http://localhost:8080/?year=2017&month=July'

´´´JavaScript
var q = url.parse(request.url, true).query;
var txt = q.year + " " + q.month;
´´´