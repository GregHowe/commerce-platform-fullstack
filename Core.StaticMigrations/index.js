(async () => {
  try {
    const { exec } = require("child_process");
    const replace = require("replace-in-file");
    const findInFiles = require("find-in-files");
    const fs = require("fs");
    const axios = require("axios");
    const args = process.argv.slice(2);

    const { uniqBy } = require("lodash");

    const config = {
      renderDir: `render`,
      scrape: true, // scrape step can optionally be skipped, to test replacement functionality
      url: "", // URL to scrape
      name: "", // Name of the site (folder to render to essentially)
      imgDelay: 500, // delay between each image download (hacky but works for now)
      routeBase: "", // set a route base for the site, so it can be served from a non-root level location on a domain
    };

    // Validate and set config items from command line args

    Object.keys(config).forEach((arg) => {
      let i = args.findIndex((a) => a === `--${arg}`) + 1;
      if (i > 0) {
        config[arg] = args[i];
      }
    });

    // ---------------------------------------------------------------

    const uri = config.url;
    const sitename = config.name.toLowerCase();
    const renderDir = `./WebCopy/${config.renderDir}/${sitename}`;

    const copyAndRunReplacements = async () => {
      console.log(`Scraping and cleaning ${uri}`);
      try {
        try {
          fs.mkdirSync(renderDir);
        } catch (err) {
          console.log(`"${renderDir}" already exists, emptying.`);
        }

        // Sometimes wcopy can't find links to certain kinds of pages, find and download them manually here.
        let root = await axios.get(uri);
        const extraLinks = root.data.match(/.[0-9]{1,45}.htm/g) || [];
        if (extraLinks.length) {
          console.log(
            "Downloading extra links (links that get missed by WebCopy)"
          );
        }
        extraLinks.forEach(async (eL) => {
          try {
            let res = await axios.get(`${uri}/${eL}`);
            if (res.status < 400) {
              fs.writeFileSync(`${renderDir}/${eL}`, res.data);
            }
          } catch (err) {
            console.log("Skipping extra link (not found): ", `${uri}/${eL}`);
          }
        });

        // Use wcopy CLI tool to scrape site
        console.log(
          "Scrape initialized. Please wait, this may take several minutes."
        );
        exec(
          `wcopy ${uri} -o ${renderDir.substring(10)} -recursive -empty -quiet`,
          { cwd: "./WebCopy" },
          async (err, stdout, stderr) => {
            if (err) {
              console.error(err);
            } else {
              console.log("Scrape complete, running Replacements");
              await runReplacements(sitename);

              // upload to blob storage ?

              console.log(`${stdout}`);
              console.log(
                stderr
                  ? stderr
                  : `Scrape Successful, your files are available in ${renderDir}`
              );
            }
          }
        );
      } catch (err) {
        console.log("Caught Error", err);
      }
    };

    // REPLACEMENTS //

    const client = require("https");
    const clientHttp = require("http");
    const path = require("path");

    // space out downloads manually to avoid misses, a bit hacky but works for now to avoid too many open handles
    async function downloadImages(images) {
      let interval = setInterval(() => {
        if (images.length) {
          let img = images.shift();
          downloadImage(img.remotePath, img.localPath);
        } else {
          clearInterval(interval);
          console.log("Done");
          process.exit(0);
        }
      }, config.imgDelay);
    }

    function downloadImage(url, filepath) {
      console.log("downloadImage", url, filepath);
      if (url.includes("https://")) {
        client.get(url, (res) => {
          res.pipe(fs.createWriteStream(filepath));
        });
      } else {
        clientHttp.get(url, (res) => {
          res.pipe(fs.createWriteStream(filepath));
        });
      }
    }

    async function runReplacements() {
      console.log(`Replacements initialized, looking in ${renderDir}`);
      try {
        // Rename all .htm to .html
        try {
          console.log(path.resolve(__dirname + renderDir.substring(1)));
          const files = await fs.readdir(
            path.resolve(__dirname + renderDir.substring(1)),
            { withFileTypes: true },
            (err, files) => {
              if (err) {
                console.log("readdir err", err);
                process.exit(1);
              }
              files.forEach(async (f) => {
                if (path.extname(f.name) === ".htm") {
                  fs.rename(
                    `${renderDir}/${f.name}`,
                    `${renderDir}/${f.name}l`,
                    (err) =>
                      console.log(
                        err ? "error " + err : `Renamed ${f.name} to ${f.name}l`
                      )
                  );
                }
              });
            }
          );
        } catch (err) {
          console.log("Couldnt rename htm to html");
        }

        // Download missing images
        const rootImageReferences = await findInFiles.find(
          /\/files\/(?:(?!("|'|&)).)*/,
          renderDir,
          ".html$"
        );
        let images = [];
        Object.keys(rootImageReferences).forEach((k) => {
          rootImageReferences[k].matches.forEach((m) => {
            let clean = m;
            let localPath = `${renderDir}${clean}`;
            let remotePath = `${uri}${clean}`;
            try {
              fs.mkdirSync(
                `${localPath.substring(0, localPath.lastIndexOf("/"))}`,
                { recursive: true }
              );
            } catch (err) {
              console.log(err);
            }
            try {
              if (!fs.existsSync(localPath)) {
                console.log(`Queueing missing image ${localPath}`);
                images.push({ remotePath, localPath });
              }
            } catch (err) {
              console.log(`Queueing missing image ${localPath}`);
              console.log(remotePath + "   /   " + localPath);
              images.push({ remotePath, localPath });
            }
          });
        });
        console.log("Processing image queue");
        try {
          images = uniqBy(images, "remotePath");
        } catch (err) {
          console.log(err);
        }
        downloadImages(images);
      } catch (err) {}
      try {
        // Perform regex replace in ripped files
        await replace({
          files: [`${renderDir}/**/*.*`, `${renderDir}/**/.*.*`],
          from: [/"index.cfm"/g, /.cfm/g, /.htm"/g],
          to: ["/index.html", ".html", '.html"'],
        });
      } catch (err) {
        console.log("Error replacing index.cfm references", err);
      }

      await fs.rename(
        `./WebCopy/${config.renderDir}/${sitename}/index.htm`,
        `./WebCopy/${config.renderDir}/${sitename}/index.html`,
        (err) => console.log("Did not need to rename index.htm to index.html")
      );
    }

    config.scrape !== "false" ? copyAndRunReplacements() : runReplacements();
  } catch (err) {
    console.error("Main level error", err);
  }
})();
