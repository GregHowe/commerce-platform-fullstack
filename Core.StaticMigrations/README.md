# Static Site Ripping Tool

## Windows only, sorry.

## Warning: regex voodoo ahead

This tool is tuned specifically to rip NYL sites developed by LMG. The sites were built in ColdFusion and have a few quirks that had to be overcome to accurately rip. The idea here is to rip sites that are too custom to build in our builder, while still being able to migrate their existing stuff over for us to host while they get sorted out.

To run:

`node runner` will automatically rip sites sequentially from sites.json. Follow the formatting to specify sitename||site URI. Please note the strange behavior that node runner will run a scrape / replace / replace. For some reason images get missed on the scrape, so it has to be run twice...only way I could get it to work. Please solve this in a PR and you will be rewarded. We are using WebCopy to do the majority of the scraping, then running our own functions to fill in as much as we can see that we missed.
