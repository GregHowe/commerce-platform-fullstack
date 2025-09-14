<template>
	<div>
		<v-chip
			small
			:color="isLive == 'LIVE' ? 'success' : 'grey darken-1'"
			:outlined="isLive == 'LIVE' ? false : true"
			:text-color="isLive == 'LIVE' ? 'white' : 'grey darken-1'"
		>
			<ForgeTooltip
				v-if="isLive === 'LIVE'"
				top
				color="black"
			>
				<template #trigger>
					<a
						class="white--text"
						:href="url"
						target="_blank"
					>
						{{ isLive }}
					</a>
				</template>
				<template #default> View site in new tab </template>
			</ForgeTooltip>
			<span v-if="isLive == 'OFF'">{{ isLive }}</span>
		</v-chip>
	</div>
</template>

<script>
import axios from "axios";
import { mapState } from "vuex";

export default {
	props: {
		item: {
			type: Object,
			default() {
				return new Object({});
			},
		},
	},
	data() {
		return {
			isLive: "",
		};
	},
	computed: {
		...mapState({
			brandId: (state) => state.brand.id,
		}),
		url() {
			return `https://f92core-nylwebsites.azureedge.net/${this.brandId}/websites/${this.item.id}/latest`;
		},
	},
	async mounted() {
		//if (!this.isLocal) {
		try {
			const res = await axios.get(this.url);
			if (res?.status && res.status < 400) {
				this.isLive = "LIVE";
			} else {
				this.isLive = "OFF";
			}
		} catch (err) {
			console.log(err);
			this.isLive = "OFF";
		}
	},
};
</script>
