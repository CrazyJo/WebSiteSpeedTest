const NODE_ENV = process.env.NODE_ENV || "production";
const webpack = require("webpack");

console.log(NODE_ENV === "dev" ? "Dev Mode" : "Production");

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
    watch: NODE_ENV === "dev",
    watchOptions: {
        aggregateTimeout: 100
    },
    devtool: "cheap-module-source-map",
    plugins: [
        new webpack.NoErrorsPlugin()
    ],
    resolve: {
        extensions: ["", "js", ".ts"]
    }
};

if (NODE_ENV !== "dev")
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