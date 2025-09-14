/* eslint-disable no-eval */
<template>
	<div class="pb-4">
		<h3 class="pa-4 pb-0">Pages</h3>
		<v-treeview
			dense
			rounded
			:items="siteTree"
		>
			<template #prepend="{ item }">
				<div v-if="item.type == 'page'">
					<!--<v-icon @click="moveUp(item.pageId)">mdi-chevron-up</v-icon>-->
					<!--<v-icon @click="moveDown(item.pageId)">mdi-chevron-down</v-icon>-->
					<v-icon
						:color="
							selectedPageId == item.pageId ? 'blue' : 'black'
						"
					>
						mdi-file-document
					</v-icon>
				</div>
				<div v-if="item.type == 'block'">
					<v-icon
						:color="
							selectedBlockIds.includes(item.id)
								? 'orange'
								: 'black'
						"
					>
						mdi-cube
					</v-icon>
				</div>
			</template>
			<template #label="{ item }">
				<div class="tree-label text-caption">
					<span
						v-if="item.type == 'block'"
						class="treelabel-block"
						@click="selectPageAndBlock(item)"
					>
						{{ item.name }}
					</span>
					<span
						v-if="item.type == 'page'"
						class="treelabel-page"
						@click="selectPage(item)"
					>
						{{ item.name }}
					</span>
				</div>
			</template>
			<template #append="{ item }">
				<div v-if="item.type == 'page'">
					<ForgeTooltip right>
						<template #trigger>
							<v-btn
								icon
								color="primary lighten-1"
								x-small
								@click="previewPage(item.pageId)"
							>
								<v-icon>mdi-eye</v-icon>
							</v-btn>
						</template>
						<span>preview this page in a new tab</span>
					</ForgeTooltip>
					<ForgeTooltip top>
						<template #trigger>
							<v-btn
								icon
								color="primary lighten-1"
								x-small
								@click="handleToggle('page')"
							>
								<v-icon>mdi-cog</v-icon>
							</v-btn>
						</template>
						<span>edit settings for this page</span>
					</ForgeTooltip>
					<ForgeTooltip top>
						<template #trigger>
							<v-btn
								icon
								color="error"
								x-small
								:disabled="isHomePage(item.pageId)"
								@click="removePageAndReroute(item.pageId)"
							>
								<v-icon>mdi-trash-can</v-icon>
							</v-btn>
						</template>
						<span>{{
							isHomePage(item.pageId)
								? "cannot delete the home page"
								: "delete this page forever"
						}}</span>
					</ForgeTooltip>
				</div>
			</template>
		</v-treeview>
		<v-btn
			class="ml-10 white--text"
			color="blue darken-1"
			x-small
			@click="createPage"
			>+ New Page</v-btn
		>
	</div>
</template>
<script>
import { mapActions, mapState, mapGetters, mapMutations } from "vuex";
export default {
	props: {
		pages: {
			type: Array,
			required: true,
		},
	},
	computed: {
		...mapState("brand", { brandCode: "code" }),
		...mapState("site", [
			"workingSite",
			"selectedSiteId",
			"selectedPageId",
			"storedSite",
			"siteTree",
			"selectedBlockIds",
		]),
		...mapGetters({
			siteTree: "site/siteTree",
		}),
	},
	methods: {
		...mapActions("site", ["createPage", "removePage"]),
		...mapMutations("site", [
			"setSelectedBlockIds",
			"setSelectedBlockId",
			"setSelectedPageId",
			"reorderPages",
		]),
		moveUp(pId) {
			const indexToMove = this.workingSite.pages.findIndex(
				(pg) => pg.id == pId
			);
			if (indexToMove > 0) {
				this.reorderPages({ p1: indexToMove, p2: indexToMove - 1 });
			}
		},
		selectPage(item) {
			this.setSelectedPageId(item.pageId);
			this.$store.commit("interface/toggleActiveSidebarKey", "page");
			this.$store.commit("site/clearSelectedBlockIds");
		},
		selectPageAndBlock(item) {
			this.setSelectedPageId(item.pageId);
			this.setSelectedBlockIds([item.id]);
			this.setSelectedBlockId(item.id);
			this.$store.commit("interface/toggleActiveSidebarKey", "block");
		},
		previewPage(page) {
			const href =
				page && this.brandCode && this.selectedSiteId
					? `https://${this.brandCode}-${this.selectedSiteId}-staging.azureedge.net/${page.slug}`
					: null;
			if (href) {
				window.open(href);
			}
		},
		handleToggle(key = null) {
			this.$store.commit("interface/toggleActiveSidebarKey", key);
			this.$store.commit("site/clearSelectedBlockIds"); // will clear selected blocks without influencing the active sidebar key
		},
		async removePageAndReroute(pageId) {
			await this.removePage(pageId);
			this.$router.push({
				path: `/site/${this.selectedSiteId}/page/${this.storedSite.pages[0].id}`,
			});
		},
		isHomePage(id) {
			return id === this.workingSite.homepageId;
		},
	},
};
</script>
