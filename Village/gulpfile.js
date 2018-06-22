/// <binding AfterBuild='default' Clean='clean' />

// Include gulp
var gulp = require('gulp');

// Include plugins for gulp into an object array
var plugins = require('gulp-load-plugins')({
    pattern: ['*', 'gulp-*', 'gulp.*'],
    replaceString: /\bgulp[\-.]/
});

// set logger
var log = require('npmlog');
log.enableColor();

// Set config for directories
var config = require('./config.json');
var packageJson = require('./package.json');
var tsconfig = require('./tsconfig.json');
var dependencies = Object.keys(packageJson && packageJson.dependencies || {});

// typescript config
var tsconfig = {
    module: 'commonjs',
    target: 'es2015',
    keepTree: false
};


/*** Internal helper functions ***/

// Error logger
var restartWatch;
function swallowError(error) {
    log.error(error.toString());
    if (restartWatch) {
        return gulp.start('watch');
    } else {
        this.emit('end');
    }
}

// Trigger
var production = false;

// Permissions
var permissions = {
    owner: {
        read: true,
        write: true,
        execute: true
    },
    group: {
        execute: true
    },
    others: {
        execute: true
    }
};


/*** Gulp development tasks ***/

// Task: Clean
// Description: Removing assets files before running other tasks
gulp.task('clean', function () {
    return gulp.src(config.assets.clean)
        .pipe(plugins.clean({ force: true }));
});

// Task: Handle SASS
gulp.task('sass', function () {
    return gulp.src(config.stylesheets.base)
        .pipe(plugins.sass())
        .on('error', swallowError)

        // TODO deze gaat fout
        //.pipe(plugins.autoprefixer({
        //    browsers: ['last 2 versions', 'ie >= 10', 'safari >= 8', 'firefox >= 41', 'chrome >= 45', 'ios >= 8', 'android >= 4.1']
        //}))

        // If producion
        .pipe(plugins.if(production, plugins.cssnano()))
        .pipe(gulp.dest(config.stylesheets.dest))
        .pipe(plugins.browserSync.reload({ stream: true }));
});

// Task: Handle Javascripts
gulp.task('javascript', function () {
    return gulp.src(config.javascripts.source)
        // .pipe(plugins.angularFilesort()) // angularjs heeft een specifieke volgorde nodig voor compilatie. TODO indien "gewone" JS en AngularJS conflicten geven moeten deze worden gescheiden.
        .pipe(plugins.concat('appbundle.js'))
        .on('error', swallowError)

        // If production
        .pipe(plugins.if(production, plugins.uglify()))
        .pipe(gulp.dest(config.javascripts.dest))
        .pipe(plugins.browserSync.reload({ stream: true }));
});

// Task: Handle Razor, which is only for the browsersync functionality
gulp.task('razor', function () {
    return gulp.src(config.razor.source)
        .pipe(plugins.browserSync.reload({ stream: true }));
});

// Task: Handle Javascript libraries
// Description: Get all libs and concat to libs.js
gulp.task('plugins_javascript', function () {
    return gulp.src(config.plugins.javascript)
        .pipe(plugins.concat('libs.js'))

        // If production
        .pipe(plugins.if(production, plugins.uglify()))
        .pipe(gulp.dest(config.javascripts.dest))
        .pipe(plugins.browserSync.reload({ stream: true }));
});
gulp.task('plugins_styles', function () {
    return gulp.src(config.plugins.styles)
        .pipe(plugins.concat('lib.css'))
        .on('error', swallowError)
        .pipe(plugins.if(production, plugins.cssnano()))
        .pipe(gulp.dest(config.stylesheets.dest));
});

// Task: Handle assets
// Description: Copy all images, videos, fonts etc to build folder
gulp.task('assets', function () {
    return gulp.start(
        'assets_js',
        'assets_css',
        'assets_fonts',
        'assets_img'
    );
});
gulp.task('assets_js', function () {
    return gulp.src(config.assets.js.source)
        .pipe(gulp.dest(config.assets.js.dest));
});
gulp.task('assets_css', function () {
    return gulp.src(config.assets.css.source)
        .pipe(gulp.dest(config.assets.css.dest));
});
gulp.task('assets_img', function () {
    return gulp.src(config.assets.img.source)
        .pipe(gulp.dest(config.assets.img.dest));
});
gulp.task('assets_fonts', function () {
    return gulp.src(config.assets.fonts.source)
        .pipe(gulp.dest(config.assets.fonts.dest));
});


// Task: BrowserSync
gulp.task("browser_sync", function () {
    return plugins.browserSync.init({
        proxy: config.dotnet.proxy
    });
});

// Task: Watch files
gulp.task('watch', ['browser_sync'], function () {
    gulp.watch(config.stylesheets.files, ['sass']); // TODO Scss wijzigingen triggeren watch wel, maar browser_sync niet
    gulp.watch(config.javascripts.source, ['javascript']);
    gulp.watch(config.razor.source, ['razor']);
}).on('error', swallowError);

// Task: Default
// Description: Build all stuff of the project once
gulp.task('default', ['clean'], function () {
    return gulp.start('javascript',
        'sass',
        'plugins_javascript',
        'assets'
    );
});

// Task: Compile
// Description: Build all dev code upon dotnet build
gulp.task('compile', function () {
    production = false;
    return gulp.start(
        'default'
    );
});

// Task: Start your production process
// Description: Build, enable browsersync and watch
gulp.task('serve', ['default'], function () {
    production = false;
    return gulp.start(
        'watch'
    );
});


// Task: TypeScript compiler for CI purposes only
var typescript = require('gulp-tsc');
gulp.task('compile_typescript', ['clean'], function () {
    return gulp.src('./components/**/*.ts')
        .pipe(typescript(tsconfig))
        .pipe(gulp.dest('./components/'));
});

// Task: Release
// Description: Build and minify
gulp.task('release:dev', ['compile_typescript'], function () {
    return gulp.start(
        'sass',
        'plugins_javascript',
        'javascript',
        'assets'
    );
});
gulp.task('release', ['release:dev'], function () {
    production = true;
    return gulp.start(
        'sass',
        'plugins_javascript',
        'javascript',
        'assets'
    );
});

/*
 * node-sass bugfix 
npm update
npm install
nodejs node_modules/node-sass/scripts/install.js
npm rebuild node-sass
 */