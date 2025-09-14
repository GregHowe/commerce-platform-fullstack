<template>
	<v-app-bar
		app
		elevation="3"
		clipped-left
		color="white"
		height="64"
	>
		<!-- eslint-disable -->
		<span class="text-h6 ml-12"
			><NuxtLink to="/">
				<v-img
					src="/img/logo-black.svg"
					max-width="120px"
					alt="core 2.0" /></NuxtLink
		></span>
		<GlobalAuthWrapper
			v-for="link in topLinks"
			:key="link.title"
			:restricted-to="link.restrictedTo"
		>
			<v-menu offset-y>
				<template v-slot:activator="{ on, attrs }">
					<v-btn
						class="white ml-4"
						v-on="on"
						text
					>
						<v-icon class="pa-0 ma-0 mr-1">{{ link.icon }}</v-icon>
						<NuxtLink
							v-show="!link.subNav.length"
							:to="{ name: link.pathName }"
							class="black--text font-weight-medium"
							>{{ link.title }}</NuxtLink
						>
						<span v-show="link.subNav.length"
							>{{ link.title
							}}<v-icon>mdi-chevron-down</v-icon></span
						>
					</v-btn>
				</template>
				<v-list v-show="link.subNav.length">
					<GlobalAuthWrapper
						v-for="sublink in link.subNav"
						:key="sublink.title"
						:restricted-to="sublink.restrictedTo"
					>
						<v-list-item>
							<v-list-item-title>
								<v-icon color="primary">{{
									sublink.icon
								}}</v-icon>
								<NuxtLink
									:to="{ name: sublink.pathName }"
									class="black--text font-weight-medium"
									>{{ sublink.title }}</NuxtLink
								></v-list-item-title
							>
						</v-list-item>
					</GlobalAuthWrapper>
				</v-list>
			</v-menu>
		</GlobalAuthWrapper>
		<!-- eslint-enable -->
		<v-spacer />
		<TopNavAvatar />
	</v-app-bar>
</template>
<script>
import { mapGetters } from "vuex";
export default {
	computed: {
		...mapGetters({
			topLinks: "nav/topLinks",
			fullName: "user/fullName",
		}),
	},
};
</script>
