<template>
	<div
		class="block__body"
		:style="{
			display: 'flex',
			alignContent: 'center',
			justifyContent: 'center',
			padding: '1rem',
		}"
	>
		<div class="block__section_content">
			<template v-if="settings.type">
				UNREGISTERED BLOCK TYPE:
				<br /><strong>{{ settings.type }}</strong>
			</template>
			<template v-else>[ Choose Block Type ]</template>
		</div>
	</div>
</template>

<script>
export default {
	name: "CoreBlockUnknown",
	props: {
		settings: {
			type: Object,
			required: true,
		},
	},
};
</script>

<style lang="scss">
.block__body {
}
</style>
