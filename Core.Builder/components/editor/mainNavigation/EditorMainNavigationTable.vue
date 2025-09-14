<template>
	<div>
		<v-dialog
			v-model="showNavTable"
			width="800"
		>
			<v-card width="800">
				<v-card-title class="mb-2">Main Navigation</v-card-title>
				<v-card-subtitle
					>This controls the navigation found in both the header and
					footer of your site. The links will appear in the order they
					appear in the table.</v-card-subtitle
				>
				<v-card-text
					><v-simple-table dense>
						<template #default>
							<thead>
								<tr>
									<th class="text-left">Label</th>
									<th class="text-left">Is Subpage</th>
									<th class="text-left">URL</th>
									<th class="text-center">
										<ForgeTooltip
											top
											color="black"
										>
											<template #trigger>
												New Tab
											</template>
											<template #default>
												Open link in a new tab.
											</template>
										</ForgeTooltip>
									</th>
									<th class="text-center">
										<ForgeTooltip
											top
											color="black"
										>
											<template #trigger>
												Folder
											</template>
											<template #default>
												If you want a nav item to have
												children, but not link to
												anything.
											</template>
										</ForgeTooltip>
									</th>
									<th class="text-left">Controls</th>
								</tr>
							</thead>
							<tbody>
								<tr
									v-for="(item, i) in navItems"
									:key="item.linkUrl + item.linkText"
								>
									<td>{{ item.linkText }}</td>
									<td>
										<v-icon
											v-if="item.parent"
											small
											color="success"
											>mdi-check</v-icon
										>
									</td>
									<td>{{ item.linkUrl }}</td>
									<td class="text-center">
										<v-icon
											v-if="item.openInNewTab"
											@click="
												setNavigationItem({
													id: item.id,
													key: 'openInNewTab',
													val: false,
												})
											"
											>mdi-tab-plus</v-icon
										>
										<v-icon
											v-else
											@click="
												setNavigationItem({
													id: item.id,
													key: 'openInNewTab',
													val: true,
												})
											"
											>mdi-tab</v-icon
										>
									</td>
									<td class="text-center">
										<v-icon
											v-if="item.isFolder"
											@click="
												setNavigationItem({
													id: item.id,
													key: 'isFolder',
													val: false,
												})
											"
											>mdi-folder</v-icon
										>
										<v-icon
											v-else
											@click="
												setNavigationItem({
													id: item.id,
													key: 'isFolder',
													val: true,
												})
											"
											>mdi-folder-outline</v-icon
										>
									</td>
									<td>
										<v-btn
											icon
											color="error"
											small
											@click="deleteNavItem(i)"
											><v-icon small
												>mdi-delete</v-icon
											></v-btn
										>
										<v-btn
											v-if="i > 0"
											icon
											small
											@click="
												moveNavItem({
													order: i,
													dir: 'down',
												})
											"
											><v-icon small
												>mdi-chevron-up</v-icon
											></v-btn
										>
										<v-btn
											v-if="i < navItems.length - 1"
											icon
											small
											@click="
												moveNavItem({
													order: i,
													dir: 'up',
												})
											"
											><v-icon small
												>mdi-chevron-down</v-icon
											></v-btn
										>
									</td>
								</tr>
							</tbody>
						</template>
					</v-simple-table></v-card-text
				>
				<v-card-actions>
					<v-btn
						class="mr-2"
						color="primary"
						@click="save"
						>Save</v-btn
					>
					<EditorMainNavigationNewItem />
					<v-spacer />
					<v-btn
						small
						outlined
						color="error"
						class="ml-2"
						@click="autoPopulate"
						>Autopopulate from page structure</v-btn
					>
					<v-btn
						small
						outlined
						color="error"
						@click="clear"
						>Clear All</v-btn
					>
				</v-card-actions>
			</v-card>
		</v-dialog>
		<v-btn
			color="warning"
			class="mb-2"
			@click="showNavTable = true"
			>Edit Navigation</v-btn
		>
	</div>
</template>
<script>
import { mapState, mapMutations } from "vuex";

export default {
	data() {
		return {
			showNavTable: false,
			showNewItemForm: false,
		};
	},
	computed: {
		...mapState({
			workingSite: (state) => state.site.workingSite,
			navItems: (state) => state.site.nav.navItems,
		}),
		rootNavItems() {
			return this.navItems?.filter((nI) => !nI.parent);
		},
	},
	mounted() {
		const navItems = this.workingSite?.navigation?.links || [];
		this.setNavigationItems(navItems);
		this.setNavItems(navItems);
	},
	methods: {
		...mapMutations({
			setNavigationItems: "site/setNavigationItems",
			insertNavItem: "site/nav/insertNavItem",
			setNavItems: "site/nav/setNavItems",
			moveNavItem: "site/nav/moveNavItem",
			deleteNavItem: "site/nav/deleteNavItem",
			setNavigationItem: "site/nav/setNavigationItem",
		}),

		save() {
			console.log(this.navItems);
			this.setNavigationItems(this.navItems);
			this.showNavTable = false;
		},
		clear() {
			this.setNavItems([]);
		},
		autoPopulate() {
			this.setNavItems([]);
			this.workingSite.pages
				.filter((p) => !p.parentPageId)
				.forEach((p) => {
					this.insertNavItem({
						linkText: p.title,
						linkUrl: `/${p.handle}`,
						isInternalLink: true,
						openInNewTab: false,
						isFolder: false,
						parent: "",
					});
					const currentNI = this.navItems.find(
						(nI) => nI.linkText == p.title
					);

					this.workingSite.pages
						.filter((pp) => pp.parentPageId == p.id)
						.forEach((pp) => {
							this.insertNavItem({
								linkText: pp.title,
								linkUrl: `/${pp.handle}`,
								isInternalLink: true,
								openInNewTab: false,
								isFolder: false,
								parent: currentNI.id,
							});
						});
				});
		},
	},
};
</script>
