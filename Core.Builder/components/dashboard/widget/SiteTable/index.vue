<template>
	<div>
		<v-data-table
			:headers="headers"
			:items="sites"
			:search="search"
			:items-per-page="12"
			item-key="id"
			class="elevation-1"
		>
			<template #top>
				<v-text-field
					v-model="search"
					label="Search Website Titles"
					class="mx-4"
					prepend-inner-icon="mdi-magnify"
				></v-text-field>
			</template>
			<template #[`item.status`]="{ item }">
				<DashboardWidgetSiteTableStatus :item="item" />
			</template>
			<template #[`item.title`]="{ item }">
				<NuxtLink
					:to="{
						name: 'site-siteId',
						params: { siteId: item.id },
					}"
				>
					{{ item.title }}
				</NuxtLink>
			</template>
		</v-data-table>
		<ForgeButton
			v-if="hasPermission('createsite')"
			class="mt-3"
			color="primary"
			@click="createSite"
		>
			<v-icon>mdi-plus</v-icon>
			Create New
		</ForgeButton>
	</div>
</template>

<script>
import { mapState, mapActions, mapGetters } from "vuex";

export default {
	props: {
		siteList: {
			type: Array,
			default() {
				return [];
			},
		},
		showUserId: {
			type: Boolean,
			default: false,
		},
	},
	data() {
		return {
			search: "",
		};
	},
	computed: {
		...mapState({
			queue: (state) => state.buildQueue.queue,
		}),
		...mapGetters({ hasPermission: "user/hasPermission" }),
		headers() {
			const items = [
				{ text: "Status", value: "status" },
				{ text: "ID", value: "id" },
				{ text: "Title", value: "title" },
			];
			if (this.showUserId) {
				items.push({ text: "Owner", value: "userId" });
			}
			return items;
		},
		sites() {
			return this.siteList.map((s) => ({
				status: this.queue.find(
					(q) =>
						q.templateParameters.CLOUDCMS_SITEID == s.id.toString()
				)
					? "PUBLISHING"
					: "LIVE",
				title: s.title,
				id: s.id,
				userId: s.userId,
			}));
		},
	},
	mounted() {
		this.GET_PIPELINE_STATUS();
	},
	methods: {
		...mapActions({
			GET_PIPELINE_STATUS: "buildQueue/GET_PIPELINE_STATUS",
			createSite: "site/createSite",
			removeSite: "site/removeSite",
		}),
		isPublishing(doc) {
			console.log(doc);
			return;
		},
	},
};
</script>
