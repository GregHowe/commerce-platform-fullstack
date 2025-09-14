<template>
	<div>
		<v-dialog
			v-model="showNewItemForm"
			width="600"
		>
			<v-card>
				<v-card-title>Add link to global navigation</v-card-title>
				<v-card-text>
					<v-container>
						<v-row
							><v-col cols="12">
								<h4>Link Options</h4>
								<v-checkbox
									v-model="navItem.isInternalLink"
									dense
									hint="Check if linking to an internal page."
									persistent-hint
									label="Internal Link"
								/>
							</v-col>
							<v-col cols="12"
								><v-text-field
									v-if="!navItem.isInternalLink"
									v-model="navItem.linkUrl"
									dense
									label="URL" />
								<v-select
									v-else
									v-model="navItem.linkUrl"
									solo
									dense
									:items="existingPages"
									label="Link to a page in your site" />
								<v-text-field
									v-model="navItem.linkText"
									dense
									label="Label"
							/></v-col>
							<v-col cols="12">
								<h4>More options</h4>
								<v-divider class="mt-1" />
							</v-col>
							<v-col cols="12">
								<v-select
									v-model="navItem.parent"
									solo
									:items="navItems"
									item-text="linkText"
									item-value="id"
									dense
									label="Select parent for subnavigation"
									hint="Link will appear nested underneath parent."
									persistent-hint
								/>
							</v-col>
							<v-col
								cols="12"
								md="6"
								><v-checkbox
									v-model="navItem.isFolder"
									dense
									label="Set as Folder"
									hint="When you don't want a nav item to link anywhere, but want it to have children."
									persistent-hint
							/></v-col>
							<v-col
								dense
								cols="12"
								md="6"
								><v-checkbox
									v-model="navItem.openInNewTab"
									dense
									label="New Tab"
									hint="Open this link in a new tab."
									persistent-hint
							/></v-col>
						</v-row>
						<v-row>
							<v-col cols="12"
								><v-btn
									color="success"
									@click="insert(navItem)"
									>Add</v-btn
								></v-col
							>
						</v-row>
					</v-container>
				</v-card-text>
			</v-card>
		</v-dialog>
		<v-btn
			color="success"
			@click="showNewItemForm = true"
			><v-icon class="mr-2">mdi-plus-box</v-icon> Add link</v-btn
		>
	</div>
</template>
<script>
import { mapState, mapMutations } from "vuex";
const defaultNavItem = {
	linkText: "",
	isInternalLink: true,
	openInNewTab: false,
	linkUrl: "",
	isFolder: false,
	parent: null,
};
export default {
	data() {
		return {
			showNewItemForm: false,
			parentNavItem: {},
			navItem: defaultNavItem,
		};
	},
	computed: {
		...mapState({
			workingSite: (state) => state.site.workingSite,
			navItems: (state) => [
				{ id: null, linkText: "(no parent)" },
				...state.site.nav.navItems,
			],
		}),
		existingPages() {
			return this.workingSite.pages.map((p) => `/${p.handle}`);
		},
	},
	mounted() {
		this.navItem = defaultNavItem;
	},
	methods: {
		...mapMutations({
			insertNavItem: "site/nav/insertNavItem",
		}),
		insert() {
			this.insertNavItem(this.navItem);
			this.navItem = defaultNavItem;
			this.showNewItemForm = false;
		},
	},
};
</script>
