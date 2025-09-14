<template>
	<div class="block__body">
		<iframe
			v-if="apiKey"
			width="600"
			height="600"
			style="border: 0"
			loading="lazy"
			allowfullscreen
			:src="mapUrl"
			:title="mapAlt"
		></iframe>
		<div v-else>API Key is required to enable this map</div>
	</div>
</template>

<script>
import { renderData } from "@libraryHelpers/dataComponents.js";

export default {
	name: "CoreBlockMap",
	props: {
		settings: {
			type: Object,
			required: true,
		},
		dataSite: {
			type: Object,
			required: true,
		},
	},
	data() {
		return {
			apiKey: process.env.NUXT_ENV_GOOGLE_MAPS_KEY,
		};
	},
	computed: {
		address() {
			return this.settings.address || {};
		},
		line1() {
			return renderData(this.address.line1, this.dataSite);
		},
		city() {
			return renderData(this.address.city, this.dataSite);
		},
		state() {
			return renderData(this.address.state, this.dataSite);
		},
		mapUrl() {
			const location = encodeURI(
				`${this.line1} ${this.city}, ${this.state}`
			);
			return `https://www.google.com/maps/embed/v1/place?q=${location}&key=${this.apiKey}`;
		},
		mapAlt() {
			return `${this.city}, ${this.state}`;
		},
	},
};
</script>

<style lang="scss">
.block_map {
	.block__body {
		width: 100%;
		& iframe {
			display: block;
			object-fit: cover;
			width: 100%;
		}
	}
}
</style>
