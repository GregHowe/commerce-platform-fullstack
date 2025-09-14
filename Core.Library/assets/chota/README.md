## Extending Chota

This is literally code lifted directly from CHOTA and loosely converted to SCSS and wrapped with a `.page` selector so it can be restricted specifically to not impact any styles in the builder that are ouside of the page previews and also so it will work in the generator

### Notes

Has some issues when being loaded in the builder, as Vuetify shares some class names and have been adding fixes as we find them.

-   removed button classes UNLESS you use the class `.button` explicitly
-   applied some styles to keep vuetify from overriding `.container`, `.row` and `.col`
