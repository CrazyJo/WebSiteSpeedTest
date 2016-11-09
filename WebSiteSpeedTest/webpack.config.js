const NODE_ENV = process.env.NODE_ENV || "development";
const webpack = require("webpack");

module.exports = {
    context: __dirname + "/Scripts/User's_JS/frontend",
    entry: {
        index: "./mainIndexView.ts"
    },
    output: {
        path: __dirname + "/Scripts/User's_JS/build/",
        filename: "[name].js"
    },
    module: {
        loaders: [
            {
                test: /\.ts$/,
                loader: "ts-loader",
                exclude: /node_modules/
            }
        ]
    },
    //watch: NODE_ENV == "development",
    watch: true,
    watchOptions: {
        aggregateTimeout: 100
    },
    //devtool: NODE_ENV == "development" ? "eval" : null,
    devtool: "cheap-module-source-map", // "cheap-module-source-map"
    plugins:[
        new webpack.NoErrorsPlugin()
        //new webpack.optimize.CommonsChunkPlugin({
        //    name: "common",
        //    chunks: ["mainIndexView"]
        //})
    ],
    resolve: {
        extensions: ["", "js",".ts"] // ["", ".js", ".ts"]
    }
};

if (NODE_ENV == "production")
{
    module.exports.plugins.push(
        new webpack.optimize.UglifyJsPlugin({
            compress: {
                warnings: false,
                drop_console: true,
                unsafe: true
            }
        }));
}