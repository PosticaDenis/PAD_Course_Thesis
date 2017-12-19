var nconf = require('nconf');


var conf = nconf.file({file: __dirname + '/../../config/db-config.json'});
var DBconf = {
    host: conf.get('host'),
    user: conf.get('user'),
    password: conf.get('password'),
    database: conf.get('database')
};

module.exports.DBconf = DBconf;
