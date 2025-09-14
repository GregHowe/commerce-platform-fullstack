<template>
	<div class="editor editor-footer">
		<v-list-item-title class="text-h6 py-2">Footer</v-list-item-title>
		<v-list dense>
			<v-list-group
				v-for="section in sections"
				:key="section.label"
				:value="false"
				class="editor-footer__group-container"
			>
				<template #activator>
					<CoreIcon
						v-if="section.icon"
						class="mr-2"
						:icon="section.icon"
					/>
					<v-list-item-title v-if="section.label">{{
						section.label
					}}</v-list-item-title>
				</template>
				<v-list-item
					v-for="schemaField in section.fields"
					v-show="isSchemaFieldShowing(schemaField)"
					:key="schemaField.key"
				>
					<component
						:is="fetchEditorMethod(schemaField.type)"
						:label="schemaField.label"
						:value="
							getWorkingSiteSetting(`footer.${schemaField.key}`)
						"
						:rules="schemaField.rules"
						:provider-name="`Footer ${schemaField.label}`"
						v-bind="schemaField.attrs"
						:fetch-editor-method="fetchEditorMethod"
						@input="
							(newValue) => {
								setWorkingSiteSetting({
									key: `footer.${schemaField.key}`,
									value: newValue,
								});
							}
						"
					/>
				</v-list-item>

				<EditorFooterDisclosures
					v-if="section.label === 'Content' && showFooter"
				/>
			</v-list-group>
		</v-list>
	</div>
</template>

<script>
import { mapGetters, mapMutations } from "vuex";
import siteSchema from "~/schemas/site.json";

export default {
	name: "EditorFooter",
	props: {
		label: {
			type: String,
			default: null,
		},
		fetchEditorMethod: {
			type: Function,
			required: true,
		},
	},
	computed: {
		...mapGetters("site", ["getWorkingSiteSetting"]),
		sections() {
			return siteSchema.fields.find((field) => field.type === "footer")
				.sections;
		},
		showFooter() {
			return this.getWorkingSiteSetting("footer.showFooter");
		},
		needsBanner() {
			// if a banner in the footer does not already exist,
			// and the site (not auth) user is a registered rep or office.
			let hasBanner = !!(
				this.getWorkingSiteSetting("footer.banner") || {}
			).bodyCopy;
			return (
				!hasBanner &&
				(this.getWorkingSiteSetting("user.isRegisteredRep") ||
					this.getWorkingSiteSetting("user.employeeType") === "GO")
			);
		},
	},
	mounted() {
		if (this.needsBanner) {
			this.setBanner();
		}
		this.setCopyright();
	},
	methods: {
		...mapMutations("site", ["setWorkingSiteSetting"]),
		setBanner() {
			// sets pencil banner to FINRA default in schemas/site.json if banner is empty
			let banner = { ...this.getWorkingSiteSetting("footer.banner") };
			const bannerSchema = this.schemaItem("banner");
			if (bannerSchema.default && !banner.bodyCopy) {
				banner = bannerSchema.default;
				this.setWorkingSiteSetting({
					key: `footer.banner`,
					value: banner,
				});
			}
		},
		setCopyright() {
			let copyright = this.getWorkingSiteSetting("footer.copyright");
			const copyrightSchema = this.schemaItem("copyright");
			if (
				copyrightSchema.default &&
				(!copyright || copyright === "<p></p>")
			) {
				copyright = copyrightSchema.default;
				this.setWorkingSiteSetting({
					key: `footer.copyright`,
					value: copyright,
				});
			}
		},
		isSchemaFieldShowing(schemaField) {
			let isShowing = true;
			if (schemaField.hidden) {
				isShowing = false;
			}
			// if this field has a requireSettings property
			// it should be an array of keys that point to other
			// fields which must have specific values for this field to show up
			if (schemaField.requireSettings) {
				schemaField.requireSettings.forEach((item) => {
					const currentValue = this.getWorkingSiteSetting(
						`footer.${item.key}`
					);
					const acceptableValues = item.values;
					if (acceptableValues.indexOf(currentValue) === -1) {
						isShowing = false; // current value is not found within the acceptable values defined in schema
					}
				});
			}
			return isShowing;
		},
		schemaItem(value) {
			return this.sections
				.find((s) => s.label === "Content")
				.fields.find((f) => f.key === value);
		},
	},
};
</script>

<style
	lang="scss"
	scoped
>
.editor-footer {
	width: 100%;
	&__group-container {
		background-color: #ffffff;
		margin: 0.7rem 0;
	}
}
</style>
