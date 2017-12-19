var gulp = require('gulp');
var clean = require('gulp-clean');
var inject = require('gulp-inject');
var concat = require('gulp-concat');

gulp.task('copyLibs', function () {

    gulp.src('./public/js/libs/**', {read: false})
        .pipe(clean());

    var libs = [
        './bower_components/angular/angular.min.js',
        './bower_components/angular-ui-router/release/angular-ui-router.min.js'
    ];

    var css = [
        './bower_components/materialize/dist/css/materialize.css',
    ];
    

    gulp.src(libs)
        .pipe(gulp.dest('./public/js/libs'))
    
    gulp.src(css)
        .pipe(gulp.dest('./public/css'))
});

gulp.task('copyApp', function () {
    var appDir = './source/app/';

    var appFiles = [
        appDir + 'app.js',
        appDir + 'routes.js',
        appDir + '**/*.js'
    ];

    return gulp.src(appFiles)
        .pipe(concat('hr.js'))
        .pipe(gulp.dest('./public/js/app'))
});

gulp.task('inject', function () {
    libsDir = './public/js/libs/';

    var target = gulp.src('./public/index.html');

    var jsFiles = gulp.src([
        libsDir + 'angular.min.js',
        libsDir + 'angular-ui-router.min.js',

        './public/js/app/hr.js'
    ]);

    return target.pipe(inject(jsFiles), {relative: true})
        .pipe(gulp.dest('./public'));
})


gulp.task('build', function () {
    gulp.start(['copyLibs', 'copyApp']);
});

gulp.task('watch', function () {
    gulp.watch(['./source/app/**'], ['copyApp'])
});
