## CoreBlocks

CoreBlocks are the pieces we use to generate a static site. Examples would be a container block, image block, text block, divider block, etc.

### CoreBlocks in Pages

Every website has one or more pages. Each of those pages has a property called `blocks` which is just a JSON string representing a tree of CoreBlocks. It is this JSON data that is used to generate the markup of the page.

### CoreBlock Types

As the generator traverses the `blocks` tree, each individual CoreBlock found within has a `type` property. This determines the CoreBlock component that will be used to generate that piece of the tree.

### CoreBlock Variants

Each individual CoreBlock also has a `variant` property. This is simple a space-delimited string of variations that should be applied to that particular CoreBlock instance. For example, a CoreBlock with `type` of `text` might have a `variant` of `center`, which would tell us it should be center aligned.

### CoreBlock Classes and Styles

Each individual CoreBlock needs class and style definitions to account for all the possible variations it might have. So that example of a `text` CoreBlock with a `variant` of `center` would end up with the following classes: `block block_text block_text_center`. It would then make a lot of sense for there to be some CSS that would center the text if that class is applied. i.e. `.block_text_center { text-align: center }`.

## Important Tips

1. Do not use Vuetify utility classes or components within CoreBlocks.
2. Do not use Vuetify variables inside the CSS for these CoreBlocks.
3. Define all the styles needed for a particular CoreBlock, including all the possible variants, within the CoreBlock style tag.
4. There are theme variables that can/should be used to make the CoreBlocks work with the site theme. _provide link to theme documentation_

### Can't We Just Use Vuetify???

Think of an application like Photoshop. It has an interface and a canvas. The pixels on a canvas have no relationship to the interface, and it is the pixels on that canvas that are ultimately exported as a static image.

In terms of our Builder, the application interface is currently built with Nuxt and Vuetify, as well as whatever customizations we have made to the theme. Then we have something like a canvas, which are the pages made up of CoreBlocks. They have their own theme, their own styles, and their own components. They have absolutely no relationship to the Builder's interface, they are the product that is ultimately exported just like that image in Photoshop.

### But the CoreBlock components are Vue files!

The ultimate goal is to migrate CoreBlocks into the CoreLibrary directory, and make them accessible to both the Builder and the Generator. Compiling the CoreBlocks at that point should build them as vanilla web components so they are usable in any context. Right now they are only being used in the Builder, so the easiest way to get them running is to just build them right here as integrated Vue files.

**REMEMBER: We are very early in development.**