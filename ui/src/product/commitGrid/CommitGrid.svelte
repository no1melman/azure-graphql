<style>

  .commit-container {
    display: grid;
    grid-template-columns: repeat(10, 1fr [col-start]);
    grid-template-rows: repeat(10, 1fr [col-start]);
    min-width: 100px;
    min-height: 100px;
  }

  .acommit {
    margin: 1px;
    background-color: #669900;
  }

  .bad-commit {
    background-color: #b22222;
  }

  .single-commit {
    grid-column: span 1;
    grid-row: span 1;
  }

  .tens-commit {
    grid-column: span 2;
    grid-row: span 1;
  }

  .hundred-commit {
    grid-column: span 2;
    grid-row: span 2;
  }

</style>

<div class="commit-container">
  <slot></slot>
</div>