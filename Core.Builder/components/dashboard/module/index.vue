<template>
	<div class="dashboard-module pa-4 ma-4">
		<h3 class="pb-4 primary--text">
			<v-icon
				class="mr-1"
				color="primary"
			>
				{{ icon }}
			</v-icon>
			{{ label }}
		</h3>
		<slot />
	</div>
</template>
<script>
export default {
	props: {
		label: {
			type: String,
			default: "",
		},
		icon: {
			type: String,
			default: "",
		},
	},
};
</script>
