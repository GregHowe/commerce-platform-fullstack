<template>
	<div class="drawer__body">
		<ForgeProgressBar :active="isBusyAnything" />
		<v-list class="my-0 py-0">
			<v-list-item class="my-0 py-0">
				<v-list-item-content class="my-0 py-0">
					<v-list-item-title>
						<span
							class="text-caption ml-12"
							style="position: relative; top: -6px"
							><NuxtLink to="/">
								<v-img
									src="/img/logo-black.svg"
									max-width="70px"
									alt="core 2.0" /></NuxtLink
						></span>
						<h1>Site Editor</h1>
						<v-btn
							x-small
							exact
							outlined
							class="my-2 mb-3"
							:to="{ path: `/site/${selectedSiteId}` }"
							><v-icon
								x-small
								class="mr-2"
								>mdi-arrow-left</v-icon
							>Return to Site Dashboard</v-btn
						>
					</v-list-item-title>
				</v-list-item-content>
			</v-list-item>
			<v-divider />
			<v-list-item>
				<v-list-item-content class="mb-0 pb-0">
					<v-list-item-title
						><h4>
							{{ siteTitle }}
						</h4></v-list-item-title
					>
				</v-list-item-content>
			</v-list-item>
			<v-list-item
				dense
				class=""
				@click="
					(e) => {
						handleToggle('site');
					}
				"
			>
				<v-list-item-icon>
					<v-icon>mdi-cog</v-icon>
				</v-list-item-icon>
				<v-list-item-content>
					<v-list-item-title>Site Settings </v-list-item-title>
				</v-list-item-content>
			</v-list-item>

			<v-list-item
				dense
				@click="
					(e) => {
						handleToggle('theme');
					}
				"
			>
				<v-list-item-icon>
					<v-icon>mdi-brush</v-icon>
				</v-list-item-icon>
				<v-list-item-content>
					<v-list-item-title>Theme Settings </v-list-item-title>
				</v-list-item-content>
			</v-list-item>
		</v-list>
		<v-divider />
		<forge-list-pages :pages="workingSite.pages" />
		<v-divider />
	</div>
</template>

<script>
import { mapState, mapGetters, mapMutations } from "vuex";
export default {
	name: "ForgeDrawer",
	computed: {
		...mapState("site", [
			"activeSidebarKey",
			"workingSite",
			"selectedSiteId",
			"selectedPageId",
			"selectedBlockIds",
			"selectedBlockId",
		]),
		...mapGetters({
			isBusyAnything: "interface/isBusyAnything",
		}),
		siteTitle() {
			return this.workingSite?.title;
		},
	},
	methods: {
		handleToggle(key = null) {
			this.$store.commit("interface/toggleActiveSidebarKey", key);
			this.$store.commit("site/clearSelectedBlockIds"); // will clear selected blocks without influencing the active sidebar key
		},
		...mapMutations("site", [
			"setSelectedBlockIds",
			"setSelectedBlockId",
			"setSelectedPageId",
		]),
	},
};
</script>
<style lang="scss">
h1 {
	font-size: 26px;
}
.v-treeview-node.v-treeview-node--rounded .v-treeview-node__root {
	margin-top: 0;
	margin-bottom: 0;
	min-height: 0;
}
.tree-label {
	cursor: pointer;
}
.tree-label:hover {
	color: purple !important;
}
</style>
