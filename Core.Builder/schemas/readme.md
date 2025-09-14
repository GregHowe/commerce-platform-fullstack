# Core Schemas

## Sites and Pages

Defines the fields available while editing a site or pages on a site. Each schema is a JSON object containing a `fields` property which is just an array of field definition objects. Each field in the array has the following properties available:

### Universal Field Properties

-   **key** is a _required_ camelCase string that maps the field's value to a property name on the object.
-   **label** is a _required_ user friendly string that labels the field's editor in the sidebar.
-   **type** is a _required_ camelCase string corresponding to the editor type used for this field in the sidebar.
-   **default** is an _optional_ value that is applied to a block when it is first added to a page. Generally speaking it is a way to get the field editing started.
-   **hint** is an _optional_ user friendly string that will appear underneath this field to nudge the user in the right direction.
-   **hidden** is an _optional_ boolean that, when true, will hide this field from the user in the sidebar.
-   **readonly** is an _optional_ boolean that, when true, will not allow the field's value to be edited in the sidebar. (if the field is hidden it does not also need to be readonly... unless you really want to drive the point home)

## Blocks

The `block` schema is a JSON object that also has a top level `fields` array which defines all of the fields that are available to _every_ block. These can use exactly the same properties as noted above, with extras:

### Block Field Properties

-   **variants** is an _optional_ array of strings that specify which block variants actually make use of this field. In other words if you set the `variants` property to `["columns", "grid"]`, then the field's editor will only be included with the block if its variant field includes either `columns` or `grid`.

## Block Types

The `block` schema also has a unique property called `types` which is an object whose kebab-cased keys represent all the different types of blocks in the system. If the `types` object has an `image` key, that would define the settings and variants available to `builder/components/blocks/CoreBlockImage`. The sidebar also looks to this schema when a specific block type is selected so it can figure out which editors to provide in addition to the universal ones outlined above.

## Block Variant Groups

Each block type can have one or more groups of variants. This is just an array of objects with the following properties:

### Block Variant Properties

-   **key** is a _required_ camelCase string that identifies a group of variant options that the user choose from. Users can only pick one variant per group.
-   **label** is a _required_ human friendly string that provides a label for this group of variant options.
-   **options** is a _required_ array of kebab-case strings that the user can pick from. When one of these options is selected, it is appended to to top element of that block as an additional class. You would then create special styles for this type of block when it has the class associated with this variant group option.

## Icons

Defines the set of icons available to users in core. This is much simpler than the others above. It is just an array of icon objects.

### Icon Properties

-   **value** is a _required_ kebab-case key that is essentially the unique id of the icon. When you want to show this icon in Core you would just add `<CoreIcon :icon="one-of-the-values" /> and it will look for an icon w/ that name. Fair warning, if it can't find that specific icon, it will load up a broken icon instead.

-   **label** is a _required_ human readable string that pretty much just lets the user see what we would call this variant if we had to call it something.

---

_TODO: do a better job of explaining or illustrating how the schema applies to core blocks, how it loads editors, and stores data which is then used to generate the pages of a site_

_TODO: pull these schemas direct from the backend models and put them somewhere we can use them in all of the projects_
