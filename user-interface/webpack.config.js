const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const CopyWebpackPlugin = require("copy-webpack-plugin");
const path = require("path");

const isProd = process.env.PROD === "true";

/** @type {import('webpack').Configuration} */
module.exports = {
  entry: path.resolve(__dirname, "src", "index.js"),
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
        test: /\.(html|svelte)$/,
        use: {
          loader: "svelte-loader",
          options: {
            emitCss: true,
          },
        },
      },
      {
        test: /node_modules\/svelte\/.*\.mjs$/,
        resolve: {
          fullySpecified: false,
        },
      },
      {
        test: /\.css$/,
        use: [MiniCssExtractPlugin.loader, "css-loader"],
      },
    ],
  },
  plugins: [
    new MiniCssExtractPlugin(),
    new CopyWebpackPlugin({
      patterns: [{ from: "index.html", to: "index.html" }],
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
