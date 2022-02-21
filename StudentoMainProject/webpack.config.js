const path = require("path");
const MomentLocalesPlugin = require('moment-locales-webpack-plugin');

module.exports = {
    entry: {

        TimeTable: "./Scripts/src/TimeTable/index.js",
        Grades: "./Scripts/src/Grades/index.js",
        SubjectDetail: "./Scripts/src/SubjectDetail/index.js",
        SubjectDetailTeacher: "./Scripts/src/SubjectDetailTeacher/index.js"

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
        ],        
    },
    plugins: [       
        new MomentLocalesPlugin({
            localesToKeep: ['cs'],
        })       
    ],
}