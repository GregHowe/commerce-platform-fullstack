const fs = require("fs");
const { spawn } = require("child_process");
// Take in a list of sites to render sequentially
let sites;
try {
  sites = fs.readFileSync("./sites.json");
  sites = JSON.parse(sites);
} catch (err) {
  console.error(
    "\x1b[31m",
    "site.json not found, please copy over sites.example.json for local use"
  );
  process.exit(1);
}

let firstSite = sites.shift().split("||");

ripSite(firstSite[0], firstSite[1]);

function ripSite(sitename, uri) {
  const child = spawn(`node index --url ${uri} --name ${sitename}`, {
    shell: true,

    env: {},
  });
  console.log("\x1b[1m\x1b[34m", `Rip started for ${sitename}`);
  child.stdout.on("data", (data) => {
    console.log("\x1b[2m\x1b[90m", `(${sitename}): ${data}`);
  });

  child.stderr.on("data", (data) => {
    console.error("\x1b[31m", `error/warning ${data}`);
  });

  child.on("close", async (code) => {
    const child2 = spawn(
      `node index --url ${uri} --name ${sitename} --scrape false`,
      {
        shell: true,

        env: {},
      }
    );
    console.log("\x1b[1m\x1b[34m", `Xtra File rip started for ${sitename}`);
    child2.stdout.on("data", (data) => {
      console.log("\x1b[2m\x1b[90m", `(${sitename}): ${data}`);
    });

    child2.stderr.on("data", (data) => {
      console.error("\x1b[31m", `error/warning ${data}`);
    });

    child2.on("close", async (code) => {
      console.log("\x1b[1m\x1b[32m", `Filerip complete: ${sitename}`);
      if (sites.length) {
        let site = sites.shift().split("||");
        ripSite(site[0], site[1]);
      }
    });
    console.log("\x1b[1m\x1b[32m", `Rip complete: ${sitename}`);
  });
}
