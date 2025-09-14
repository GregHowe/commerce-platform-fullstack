<script>
import axios from "axios";
export default {
	props: {
		url: {
			type: String,
			default: "",
		},
		env: {
			type: String,
			default: "",
		},
	},
	data() {
		return {
			status: "LOADING",
			statusIcons: {
				LOADING: {
					icon: "mdi-reload",
					color: "blue",
					tooltip: "Checking site status",
				},
				ERROR: {
					icon: "mdi-power-plug-off-outline",
					color: "grey",
					tooltip: "This environment is not online",
				},
				LIVE: {
					icon: "mdi-check-bold",
					color: "green",
					tooltip: "All systems go!",
				},
				LOCAL: {
					// CORS will prevent this from working locally
					icon: "mdi-help-network",
					color: "blue darken-3",
					tooltip: "Cannot check status due to CORS",
				},
			},
		};
	},
	computed: {
		isLocal() {
			if (process.client) {
				return window && window.location.href.includes("localhost");
			}
			return false;
		},
		statusIconClass() {
			if (this.isLocal) {
				return "LOCAL";
			}
			return this.status != "LOADING"
				? "status-icon"
				: "status-icon status-icon-active";
		},
	},
	async mounted() {
		try {
			const res = await axios.get(this.url);

			if (res?.status && res.status < 400) {
				this.status = "LIVE";
			} else {
				this.status = "ERROR";
			}
		} catch (err) {
			console.log(err);
			this.status = "ERROR";
		}
	},
};
</script>
<template>
	<div class="mr-4 mb-4">
		<ForgeTooltip
			top
			color="black"
		>
			<template #trigger>
				<v-icon
					style="cursor: pointer"
					:class="statusIconClass"
					:color="statusIcons[status].color"
					>{{ statusIcons[status].icon }}</v-icon
				>
			</template>
			<template #default>
				{{ statusIcons[status].tooltip }}
			</template>
		</ForgeTooltip>
		<ForgeTooltip
			top
			color="black"
		>
			<template #trigger>
				<a
					:href="url"
					target="_blank"
				>
					<ForgeChip
						small
						outlined
						:name="env"
						color="blue darken-3"
						class="env-chip d-inline font-weight-bold"
					/>
				</a>
			</template>
			<template #default>
				{{ url }}
			</template>
		</ForgeTooltip>
	</div>
</template>
<style
	scoped
	lang="scss"
>
.link-container {
	display: inline-block;
	padding: 4px 6px;
	margin: 1px 0;
}
.env-chip {
	text-transform: uppercase;
}
.status-icon {
	opacity: 1;
}
.status-icon-active {
	opacity: 0;
	transition: opacity 2s;
}
</style>
