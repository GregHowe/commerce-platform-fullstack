<template>
	<div class="forge forge-chip">
		<v-chip
			v-bind="$attrs"
			v-on="$listeners"
		>
			<v-avatar
				v-if="icon"
				left
			>
				<v-icon :color="icon.color"> {{ icon.name }}</v-icon>
			</v-avatar>
			{{ $attrs.name }}
		</v-chip>
	</div>
</template>

<script>
export default {
	name: "ForgeChip",
	inheritAttrs: false,
	props: {
		icon: {
			type: Object,
			default: null,
		},
	},
};
</script>
