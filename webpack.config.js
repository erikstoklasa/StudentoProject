const path = require("path");

module.exports = {
    entry: {        
        Grades: "./Scripts/src/Grades/index.js",
        TimeTable: "./Scripts/src/TimeTable/index.js"
    },
    output: {
        path: path.resolve(__dirname, "./wwwroot/dist"),
        filename: "[name].js"
    },
    module: {
        rules: [
            {
                use: {
                    loader: "babel-loader"
                },
                test: /\.js$/,
                exclude: /node_modules/ //excludes node_modules folder from being transpiled by babel. We do this because it's a waste of resources to do so.
            },
            {
                test: /\.css$/,
                use: ["style-loader", "css-loader"],
                exclude: /node_modules/
            },
        ]
    }
}