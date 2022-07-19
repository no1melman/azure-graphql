<style>
  .container {
    width: 100vw;
    height: 100vh;
    display: flex;
    overflow: none;
  }
  .navigation {
    width: 200px;
    display: flex;
    background-color: #393e60;
    color: #fff;
    font-size: 0.7rem;
  }
  .main {
    position: relative;
  }
  :global([slot="main"]),
  .main {
    flex: 1;
    display: flex;
    overflow-y: auto;
    justify-content: flex-start;
    align-items: stretch;
    flex-flow: column nowrap;
  }
  :global([slot="main"]) {
    padding: 8px;
  }
  :global([slot="navigation"]) {
    flex: 1;
    display: flex;
    overflow-y: auto;
    justify-content: flex-start;
    align-items: stretch;
    flex-flow: column nowrap;
  }
</style>

<div class="container">
  <div class="navigation">
    <slot name="navigation" />
  </div>
  <div class="main">
    <slot name="main" />
  </div>
</div>