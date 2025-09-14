const welcomeScreen = async (context) => {
	context.res = {
		headers: {
			"Content-Type": "text/html",
		},
		body: `
<html>
	<head>
		<title>Core2.0 SSO Test: Welcome</title>
		<style>
			body { font-family: sans-serif; padding: 2rem; }
			hr {
				margin: 1rem 0 1.5rem;
			}
			.button {
				display: inline-flex;
				padding: 0 1rem;
				border-radius: .4rem;
				text-transform: uppercase;
				font-weight: 600;
				align-items: center;
				background: white;
				border: 1px solid rgb(25, 118, 210);
				color: rgb(25, 118, 210);
				font-size: 14px;
				height: 44px;
				cursor: pointer;
				text-decoration: none;
			}
		</style>
	</head>
	<body>
		<h1>SSO Test: Welcome</h1>
		<hr>
		<a class="button" href="${process.env.NYLSSO_URL}" title="Log in with your NYL account.">
			Continue With NYL Account
		</a>
	</body>
</html>
`,
		status: 200,
	};
};

module.exports = welcomeScreen;
