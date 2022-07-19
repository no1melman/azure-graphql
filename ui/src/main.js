import App from './App.svelte';

import 'normalize.css'
import { getAllCommits } from './azure/commits';

getAllCommits()


const app = new App({
	target: document.body,
	props: {
		name: 'world'
	}
});

export default app;