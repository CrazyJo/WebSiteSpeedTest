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
    watch: false,
    watchOptions: {
        aggregateTimeout: 100
    },
    devtool: "source-map",
    plugins: [
        new webpack.NoErrorsPlugin()
    ],
    resolve: {
        extensions: ["", "js", ".ts"]
    }
};

module.exports.plugins.push(
    new webpack.optimize.UglifyJsPlugin({
        compress: {
            warnings: false,
            drop_console: true,
            unsafe: true
        }
    }));
