# village

Notes:

installing thru NPM will result in a BUG. To fix this do the following:
```
npm update
npm install
node node_modules/node-sass/scripts/install.js
npm rebuild node-sass
```

Then run migrations to generate a database:
```
dotnet ef database update -v
```
the -v options is the detect possible issues's

When all set, start the app from Visual Studio and run 'Gulp Serve'.
Gulp serve will start browsersync, which in term, will start the browser using a proxy.

