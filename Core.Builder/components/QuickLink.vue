<template>
	<div class="rounded pa-4 grey lighten-4">
		<div class="font-weight-bold">
			<v-icon
				color="black"
				class="mr-2"
			>
				{{ quicklink.icon }}
			</v-icon>
			{{ quicklink.title }}
		</div>
		<div class="description py-3">
			{{ quicklink.description }}
		</div>
		<div class="font-weight-bold">
			<NuxtLink
				class="quicklink primary--text"
				:to="quicklink.link"
			>
				{{ quicklink.label }}
			</NuxtLink>
			<v-icon color="black">mdi-chevron-right</v-icon>
		</div>
	</div>
</template>
<script>
export default {
	props: {
		quicklink: {
			type: Object,
			default: () => ({
				icon: "",
				title: "",
				description: "",
				label: "",
				link: {
					name: "",
					params: {
						siteId: "",
					},
				},
			}),
		},
	},
};
</script>
<style
	lang="scss"
	scoped
>
.description {
	font-size: 14px;
	line-height: 21px;
	min-height: 75px;
}
</style>
