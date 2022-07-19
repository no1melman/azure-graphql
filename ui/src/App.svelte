<script>
	
	import { Router, Route, Link } from "yrv";

	import Layout from "./Layout.svelte";

	/*==== PAGES ==== */
	import Home from './Home.svelte'
	import Product from './product/Product.svelte'
  import Navigation from "./Navigation.svelte";

</script>

<style>
  main {
    text-align: center;
    padding: 1em;
    max-width: 240px;
    margin: 0 auto;
  }
  h1 {
    color: #ff3e00;
    text-transform: uppercase;
    font-size: 4em;
    font-weight: 100;
  }
  @media (min-width: 640px) {
    main {
      max-width: none;
    }
  }
</style>

<Layout>
  <div slot="navigation">
    <Navigation />
  </div>
  <div slot="main">
    <Router>
      <Route exact path="/" component={Home} />
      <Route exact path="/product" component={Product} />
    </Router>
  </div>
</Layout>