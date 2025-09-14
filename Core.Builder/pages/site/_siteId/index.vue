<template>
	<v-container
		v-if="workingSite"
		fluid
		class="pa-0"
	>
		<v-parallax
			dark
			src="/img/rectanglebg.jpg"
			height="240"
			class="px-4"
		>
			<v-row
				align="center"
				justify="center"
			>
				<v-col
					class="text-left px-12"
					cols="12"
				>
					<h1 style="font-size: 42px">
						<v-icon
							style="top: -5px"
							color="white"
							size="42"
						>
							mdi-web
						</v-icon>
						{{ workingSite.title }}
					</h1>
					<h4 class="subheading">Website Dashboard</h4>
				</v-col>
			</v-row>
		</v-parallax>

		<v-container fluid>
			<v-row justify="start">
				<v-col
					cols="12"
					lg="8"
				>
					<DashboardModule
						label="Quick Links"
						icon="mdi-link-variant"
					>
						<v-row>
							<v-col
								v-for="q in quicklinks"
								:key="q.title"
								cols="12"
								md="6"
							>
								<QuickLink :quicklink="q" />
							</v-col>
						</v-row>
					</DashboardModule>
				</v-col>
				<v-col
					cols="12"
					lg="4"
				>
					<DashboardModule
						label="Site Environments"
						icon="mdi-earth"
					>
						<SiteEnvironments />
					</DashboardModule>
					<v-divider></v-divider>
					<GlobalAuthWrapper :restricted-to="['Admin']">
						<DashboardModule
							label="Generate"
							icon="mdi-cloud-upload-outline"
						>
							<div class="mb-2">
								Request a generation of your staging site.
							</div>
							<v-btn
								outlined
								color="primary"
								small
								@click="generateStaging"
								><span>Regenerate</span>
							</v-btn>
						</DashboardModule>
					</GlobalAuthWrapper>
					<v-divider></v-divider>
					<DashboardModule
						v-show="isQueued"
						label="Publishing Status"
						icon="mdi-list-status"
					>
						<BuildQueue class="mt-lg-10" />
					</DashboardModule>
					<DashboardModule
						label="Notifications"
						icon="mdi-bell-outline"
						><v-virtual-scroll
							:bench="benched"
							:items="items"
							height="300"
							item-height="80"
						>
							<template #default="{ item }">
								<v-list-item
									:key="item.title"
									class="pl-0"
								>
									<v-list-item-content>
										<v-list-item-title class="mb-2">
											<v-icon
												class="mr-3"
												color="black"
											>
												{{ item.icon }}
											</v-icon>
											<strong>{{ item.title }}</strong>
										</v-list-item-title>
										<v-list-item-subtitle
											class="text-caption"
										>
											{{ item.description }}
										</v-list-item-subtitle>
									</v-list-item-content>

									<v-list-item-action>
										<v-icon color="black">
											mdi-chevron-right
										</v-icon>
									</v-list-item-action>
								</v-list-item>

								<v-divider></v-divider>
							</template>
						</v-virtual-scroll>
					</DashboardModule>
					<GlobalAuthWrapper :restricted-to="['Admin']">
						<DashboardModule
							label="Delete Site"
							icon="mdi-delete-forever"
						>
							<div class="pl-1">
								<div class="text-caption mb-1">
									This action cannot be undone.
								</div>
								<v-btn
									outlined
									color="error"
									small
									@click="performSiteRemoval"
									><span>Remove</span>
								</v-btn>
							</div>
						</DashboardModule>
					</GlobalAuthWrapper>
				</v-col>
			</v-row>
		</v-container>
	</v-container>
</template>

<script>
/*
TODO move alerts to the vuex interface store
so we can set them anywhere
*/
import { mapGetters, mapMutations, mapActions, mapState } from "vuex";
export default {
	layout: "site",
	data() {
		return {
			alert: null,
			benched: 0,
			items: [
				{
					icon: "mdi-account-plus-outline",
					title: "2 USERS ADDED",
					description: "Sara Kind and John Bend have been added",
				},
				{
					icon: "mdi-filmstrip",
					title: "15 NEW ASSETS ADDED",
					description: "Sara Kind and John Bend have been added",
				},
				{
					icon: "mdi-cart-outline",
					title: "CORE 2.0 SUBSCRIPTION",
					description: "Monthly Renewal",
				},
				{
					icon: "mdi-poll",
					title: "REPORTING",
					description: "New high visitor report",
				},
				{
					icon: "mdi-filmstrip",
					title: "15 NEW ASSETS ADDED",
					description: "Sara Kind and John Bend have been added",
				},
				{
					icon: "mdi-cart-outline",
					title: "CORE 2.0 SUBSCRIPTION",
					description: "Monthly Renewal",
				},
			],
		};
	},
	computed: {
		...mapState("site", ["workingSite"]),
		...mapState({
			brandId: (state) => state.brand.id,
		}),
		...mapGetters("site", ["hasWorkingChanges"]),
		...mapGetters("buildQueue", ["isQueued"]),

		hasAlert() {
			return !!this.alert;
		},
		inputSiteTitle: {
			get() {
				return this.workingSite.title;
			},
			set(newVal) {
				this.extendWorkingSite({
					title: newVal,
				});
				this.resetWorkingPageAndBlock();
			},
		},
		quicklinks() {
			if (this?.workingSite?.id) {
				return [
					{
						icon: "mdi-monitor-cellphone-star",
						title: "MY WEBSITE",
						description:
							"Manage your website including page management, site style, and themes",
						label: "Edit My Website",
						link: {
							name: "site-siteId-page-pageId",
							params: {
								siteId: this?.workingSite?.id || "",
								pageId: this?.workingSite?.pages[0].id || "",
							},
						},
					},
					{
						icon: "mdi-poll",
						title: "REPORTING",
						description:
							"View and manage your sites analytics and generate reports",
						label: "View My Analytics",
						link: {
							name: "site-siteId-analytics",
							params: {
								siteId: this?.workingSite?.id || "",
							},
						},
					},
					{
						icon: "mdi-tray-arrow-up",
						title: "ADD CONTENT",
						description:
							"Upload your own images, videos and articles",
						label: "Upload My Content",
						link: {
							name: "site-siteId-assets",
							params: {
								siteId: this?.workingSite?.id || "",
							},
						},
					},
					{
						icon: "mdi-web",
						title: "MY DOMAINS",
						description:
							"View and manage your current domains, purchase a domain, and connect domains.",
						label: "Manage Domains",
						link: {
							name: "site-siteId-analytics",
							params: {
								siteId: this?.workingSite?.id || "",
							},
						},
					},
					{
						icon: "mdi-cog",
						title: "SETTINGS",
						description:
							"Manage your site settings including domains and integrations",
						label: "View Site Settings",
						link: {
							name: "site-siteId-analytics",
							params: {
								siteId: this?.workingSite?.id || "",
							},
						},
					},
				];
			}
			return [];
		},
	},
	methods: {
		...mapActions("site", ["removeSite"]),
		...mapMutations("site", [
			"extendWorkingSite",
			"resetWorkingPageAndBlock",
		]),
		generateStaging() {
			console.log(this.$axiosNode);
			this.$axiosNode.get(
				`/testGenerate?brandId=${this.brandId}&siteId=${this.workingSite?.id}`
			);
		},
		async performSiteRemoval() {
			await this.removeSite(this.workingSite.id);
			this.$router.push({ path: "/" });
		},
	},
};
</script>
<style
	lang="scss"
	scoped
>
.herositename {
	font-size: 12px;
}
</style>
