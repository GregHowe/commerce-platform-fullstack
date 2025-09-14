<template>
	<v-tooltip
		class="forgetooltip"
		v-bind="$attrs"
		:color="$attrs.color || 'primary'"
	>
		<template #activator="{ on, attrs }">
			<span
				v-bind="attrs"
				v-on="on"
			>
				<slot name="trigger"></slot>
			</span>
		</template>
		<slot name="default"></slot>
	</v-tooltip>
</template>

<script>
export default {
	name: "ForgeTooltip",
};
</script>
