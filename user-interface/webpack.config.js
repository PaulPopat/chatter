const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyWebpackPlugin = require("copy-webpack-plugin");
const VirtualModulesPlugin = require("webpack-virtual-modules");
const path = require("path");

const isProd = process.env.PROD === "true";

const config = {
  SSO_BASE: process.env.SSO_BASE_URL,
};

/** @type {import('webpack').Configuration} */
module.exports = {
  entry: path.resolve(__dirname, "src", "index.ts"),
  output: {
    publicPath: "/_/",
    path: path.join(__dirname, "dist"),
    filename: "main.js",
  },
  devtool: "source-map",
  resolve: {
    alias: {
      svelte: path.resolve(__dirname, "node_modules", "svelte/src/runtime"),
    },
    extensions: [".mjs", ".js", ".svelte"],
    mainFields: ["svelte", "browser", "module", "main"],
    conditionNames: ["svelte", "browser", "import"],
  },
  module: {
    rules: [
      {
        test: /\.tsx?$/,
        use: "ts-loader",
        exclude: /node_modules/,
      },
      {
        test: /\.css$/,
        use: [MiniCssExtractPlugin.loader, "css-loader"],
      },
      {
        test: /\.svg$/,
        loader: "svg-inline-loader",
      },
    ],
  },
  resolve: {
    alias: {
      "react-native$": "react-native-web",
    },
    extensions: [".tsx", ".ts", ".js"],
  },
  plugins: [
    new MiniCssExtractPlugin(),
    new CopyWebpackPlugin({
      patterns: [{ from: "index.html", to: "index.html" }],
    }),
    new VirtualModulesPlugin({
      "node_modules/@effuse/config.js": `module.exports = ${JSON.stringify(
        config
      )};`,
    }),
  ],
  mode: isProd ? "production" : "development",
  ...(!isProd
    ? {
        devServer: {
          port: 3001,
          historyApiFallback: {
            index: "/_/index.html",
          },
        },
      }
    : {}),
  watch: !isProd,
};
