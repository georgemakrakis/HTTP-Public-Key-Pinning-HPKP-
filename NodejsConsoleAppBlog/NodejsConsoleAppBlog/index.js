var express = require('express');
var https = require('https');
var fs = require('fs');
var path = require('path');

var app = express();

var certOptions = {
    key: fs.readFileSync(path.resolve('build/certs/server.key')),
    cert: fs.readFileSync(path.resolve('build/certs/server.crt'))
};

var server = https.createServer(certOptions, app);

app.get('/', function(req, res) {
    res.send('Hello world');
});

server.listen(3000);

module.exports = app;
