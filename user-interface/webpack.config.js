// @ts-check
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const HtmlWebpackPlugin = require("html-webpack-plugin");
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
    path: path.join(__dirname, "dist"),
    filename: '[hash].js',
    chunkFilename: '[chunkhash].js',
  },
  devtool: "source-map",
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
    new HtmlWebpackPlugin({
      title: "Effuse",
      inject: false,
      meta: {
        viewport:
          "width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no",
      },
      templateContent: ({ htmlWebpackPlugin }) => `
        <html>
          <head>
            ${htmlWebpackPlugin.tags.headTags}
            <style>
              html,
              body {
                height: 100%;
              }
              body {
                overflow: hidden;
              }
              #root {
                display: flex;
                height: 100%;
              }
            </style>
          </head>
          <body>
            <div id="root"></div>
            ${htmlWebpackPlugin.tags.bodyTags}
          </body>
        </html>
      `,
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
            index: "index.html",
          },
        },
      }
    : {}),
  watch: !isProd,
};
