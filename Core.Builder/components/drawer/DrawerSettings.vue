<template>
	<ForgeDrawer
		v-if="activeComponent"
		app
		right
		:style="{
			'min-width': settingsWidth,
			transition: 'min-width .2s ease-out',
		}"
	>
		<template #prepend>
			<v-row>
				<v-spacer></v-spacer>
				<v-col cols="auto">
					<v-btn
						x-small
						icon
						@click="toggleMaximizeSettings"
					>
						<v-icon color="black">mdi-window-maximize</v-icon>
					</v-btn>
					<v-btn
						x-small
						icon
						@click="closeSettings"
					>
						<v-icon color="black">mdi-close</v-icon>
					</v-btn>
				</v-col>
			</v-row>
		</template>
		<component :is="activeComponent" />
	</ForgeDrawer>
</template>

<script>
/*

The sidebar on the right displays settings
whenever a user interacts with something that
can be edited, and can also be closed.

*/

import DrawerSettingsBlock from "~/components/drawer/DrawerSettingsBlock.vue";
import DrawerSettingsPage from "~/components/drawer/DrawerSettingsPage.vue";
import DrawerSettingsSite from "~/components/drawer/DrawerSettingsSite.vue";
import DrawerSettingsTheme from "~/components/drawer/DrawerSettingsTheme.vue";

import { mapMutations, mapState, mapGetters } from "vuex";
export default {
	computed: {
		...mapState("site", [
			"selectedPageId",
			"selectedSiteId",
			"selectedThemeId",
			"workingBlock",
			"selectedBlockId",
			"selectedBlockIds",
		]),
		...mapState("interface", ["activeSidebarKey", "isSettingsMaximized"]),
		activeComponent() {
			if (this.$auth.user) {
				switch (this.activeSidebarKey) {
					case "block":
						return DrawerSettingsBlock;
					case "page":
						return DrawerSettingsPage;
					case "site":
						return DrawerSettingsSite;
					case "theme":
						return DrawerSettingsTheme;
				}
			}
			return null;
		},
		...mapGetters({
			hasWorkingChanges: "site/hasWorkingChanges",
			siteTree: "site/siteTree",
		}),
		settingsWidth() {
			// hard coded a figure in to keep the left sidebar visible
			return this.isSettingsMaximized ? "1100px" : "270px";
		},
	},
	methods: {
		...mapMutations("site", ["setSelectedBlockId", "setSelectedPageId"]),
		...mapMutations("interface", [
			"setActiveSidebarKey",
			"toggleMaximizeSettings",
		]),
		closeSettings() {
			this.setActiveSidebarKey();
		},
		selectPageAndBlock(item) {
			console.log(item);
			this.setSelectedPageId(item.pageId);
			this.setSelectedBlockId(item.id);
		},
	},
};
</script>
