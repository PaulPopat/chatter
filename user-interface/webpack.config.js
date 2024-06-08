// @ts-check
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const HtmlWebpackPlugin = require("html-webpack-plugin");
const VirtualModulesPlugin = require("webpack-virtual-modules");
const path = require("path");
const { favicons } = require("favicons");
const { Compilation, sources } = require("webpack");
const CopyPlugin = require("copy-webpack-plugin");

const isProd = process.env.PROD === "true";

const config = {
  SSO_BASE: process.env.SSO_BASE_URL,
};

/** @type {() => Promise<import('webpack').Configuration>} */
module.exports = async () => {
  const response = await favicons(
    path.resolve(__dirname, "asset-src/app-icon.png"),
    {
      appName: "Effuse",
      appShortName: "Effuse",
      appDescription: "The privacy focused chat for gamers",
      developerName: "Ipheion",
      developerURL: "https://github.com/Iph3i0n",
      lang: "en-GB",
      background: "#f8f9f9",
      theme_color: "#f8f9f9",
      appleStatusBarStyle: "black-translucent",
      display: "standalone",
      orientation: "portrait",
      scope: "/",
      start_url: "https://app.effuse.cloud/",
      version: "1.0",
    }
  );

  return {
    entry: path.resolve(__dirname, "src", "index.ts"),
    output: {
      path: path.join(__dirname, "dist"),
      filename: "[hash].js",
      chunkFilename: "[chunkhash].js",
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
      new CopyPlugin({
        patterns: [
          {
            from: "node_modules/remixicon/fonts/remixicon.ttf",
            to: "icons.ttf",
          }
        ],
      }),
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
            ${response.html.join("")}
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

              @font-face {
                src: url(/icons.ttf);
                font-family: 'RemixIcon';
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
      async (compiler) => {
        compiler.hooks.thisCompilation.tap("Copy Icons", (compilation) => {
          compilation.hooks.processAssets.tap(
            {
              name: "Copy Icons",
              stage: Compilation.PROCESS_ASSETS_STAGE_SUMMARIZE,
            },
            async (assets) => {
              for (const file of response.files)
                compilation.emitAsset(
                  file.name,
                  new sources.RawSource(file.contents)
                );

              for (const image of response.images)
                compilation.emitAsset(
                  image.name,
                  new sources.RawSource(image.contents)
                );
            }
          );
        });
      },
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
};
