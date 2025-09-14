<template>
	<v-container>
		<v-row>
			<v-col
				cols="12"
				md="8"
			>
				<h1>My websites</h1>
				<p>Select a site to edit, view, or open its dashboard.</p>
			</v-col>
			<v-col
				cols="12"
				md="4"
				class="text-right"
			>
				<v-btn
					class="mt-3"
					color="primary"
					@click="createSite"
					><v-icon>mdi-plus</v-icon> Create New</v-btn
				>
			</v-col>
			<v-col
				v-for="site in siteList"
				:key="site.id"
				cols="12"
				md="6"
				lg="4"
			>
				<v-card>
					<NuxtLink :to="`/site/${site.id}`">
						<v-img
							height="250"
							src="https://cdn.vuetifyjs.com/images/cards/cooking.png"
						></v-img>
					</NuxtLink>
					<v-card-title>{{ site.title }}</v-card-title>
					<v-card-text>Live</v-card-text>
					<v-card-actions
						><v-btn
							icon
							:to="`/site/${site.id}`"
							><v-icon color="primary"
								>mdi-playlist-edit</v-icon
							></v-btn
						>
						<v-spacer />
						<v-btn
							icon
							@click="removeSite(site.id)"
							><v-icon color="error">mdi-delete</v-icon></v-btn
						>
					</v-card-actions>
				</v-card>
			</v-col>
		</v-row>
	</v-container>
</template>

<script>
import { mapActions, mapState } from "vuex";

export default {
	layout: "headeronly",
	computed: {
		...mapState("site", ["siteList"]),
	},

	methods: {
		...mapActions("site", ["createSite", "removeSite"]),
	},
};
</script>
